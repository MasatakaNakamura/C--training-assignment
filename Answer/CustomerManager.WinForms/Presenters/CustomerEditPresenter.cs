using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;
using CustomerManager.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.WinForms.Presenters
{
    /// <summary>
    /// 顧客編集画面のPresenter
    /// ViewとModelの仲介役
    /// MVPパターンのP（Presenter）部分
    /// </summary>
    public class CustomerEditPresenter
    {
        private readonly ICustomerEditView _view;
        private readonly ICustomerRepository _repository;
        private readonly ValidationService _validationService;
        private Customer? _originalCustomer;

        public CustomerEditPresenter(ICustomerEditView view, ICustomerRepository repository)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validationService = new ValidationService();

            // Viewのイベントを購読
            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

        /// <summary>
        /// 編集対象の顧客を設定
        /// </summary>
        /// <param name="customer">編集する顧客</param>
        public void SetCustomer(Customer customer)
        {
            _originalCustomer = customer;
            _view.SetCustomer(customer);
        }

        /// <summary>
        /// 保存要求時の処理
        /// </summary>
        private async void OnSaveRequested(object? sender, EventArgs e)
        {
            await SaveCustomerAsync();
        }

        /// <summary>
        /// キャンセル要求時の処理
        /// </summary>
        private void OnCancelRequested(object? sender, EventArgs e)
        {
            _view.CloseDialog();
        }

        /// <summary>
        /// 顧客データを保存
        /// </summary>
        private async Task SaveCustomerAsync()
        {
            try
            {
                _view.SetLoading(true);
                _view.ClearErrors();

                // 画面から入力データを取得
                var customer = _view.GetCustomer();

                // バリデーション実行
                var validationResult = _validationService.ValidateCustomer(customer);
                if (!validationResult.IsValid)
                {
                    // バリデーションエラーを表示
                    foreach (var error in validationResult.Errors)
                    {
                        _view.ShowFieldError(error.Key, error.Value);
                    }
                    return;
                }

                // メールアドレスの重複チェック
                int? excludeId = _view.IsEditMode ? _originalCustomer?.Id : null;
                bool emailExists = await _repository.EmailExistsAsync(customer.Email, excludeId);
                if (emailExists)
                {
                    _view.ShowFieldError("Email", "このメールアドレスは既に使用されています。");
                    return;
                }

                // データベースに保存
                if (_view.IsEditMode && _originalCustomer != null)
                {
                    // 更新処理
                    customer.Id = _originalCustomer.Id;
                    customer.CreatedAt = _originalCustomer.CreatedAt; // 作成日時は保持
                    await _repository.UpdateAsync(customer);
                    _view.ShowSuccess("顧客情報を更新しました。");
                }
                else
                {
                    // 新規登録処理
                    await _repository.AddAsync(customer);
                    _view.ShowSuccess("顧客を登録しました。");
                }

                // 成功時はダイアログを閉じる
                await Task.Delay(1000); // 成功メッセージを表示するための待機
                _view.CloseDialog();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate entry") == true)
            {
                // MySQL固有のエラーハンドリング
                _view.ShowFieldError("Email", "このメールアドレスは既に使用されています。");
            }
            catch (Exception ex)
            {
                _view.ShowError("顧客情報の保存中にエラーが発生しました。\n再度お試しください。");
                // TODO: ログ出力
                Console.WriteLine($"Error saving customer: {ex}");
            }
            finally
            {
                _view.SetLoading(false);
            }
        }
    }
}