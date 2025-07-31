using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;

namespace CustomerManager.WinForms.Presenters
{
    /// <summary>
    /// 顧客一覧画面のPresenter
    /// ViewとModelの仲介役
    /// MVPパターンのP（Presenter）部分
    /// </summary>
    public class CustomerListPresenter
    {
        private readonly ICustomerListView _view;
        private readonly ICustomerRepository _repository;

        public CustomerListPresenter(ICustomerListView view, ICustomerRepository repository)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            // Viewのイベントを購読
            _view.LoadRequested += OnLoadRequested;
            _view.AddNewRequested += OnAddNewRequested;
            _view.EditRequested += OnEditRequested;
            _view.DeleteRequested += OnDeleteRequested;
            _view.RefreshRequested += OnRefreshRequested;
        }

        /// <summary>
        /// 初期化処理
        /// アプリケーション起動時にデータを読み込む
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadCustomersAsync();
        }

        /// <summary>
        /// データ読み込み要求時の処理
        /// </summary>
        private async void OnLoadRequested(object? sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        /// <summary>
        /// 新規登録要求時の処理
        /// </summary>
        private void OnAddNewRequested(object? sender, EventArgs e)
        {
            ShowCustomerEditDialog(null);
        }

        /// <summary>
        /// 編集要求時の処理
        /// </summary>
        private void OnEditRequested(object? sender, EventArgs e)
        {
            var selectedCustomer = _view.GetSelectedCustomer();
            if (selectedCustomer == null)
            {
                _view.ShowError("編集する顧客を選択してください。");
                return;
            }

            ShowCustomerEditDialog(selectedCustomer);
        }

        /// <summary>
        /// 削除要求時の処理
        /// </summary>
        private async void OnDeleteRequested(object? sender, EventArgs e)
        {
            var selectedCustomer = _view.GetSelectedCustomer();
            if (selectedCustomer == null)
            {
                _view.ShowError("削除する顧客を選択してください。");
                return;
            }

            // 削除確認
            if (!_view.ConfirmDelete(selectedCustomer.Name))
            {
                return;
            }

            await DeleteCustomerAsync(selectedCustomer.Id);
        }

        /// <summary>
        /// 更新要求時の処理
        /// </summary>
        private async void OnRefreshRequested(object? sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        /// <summary>
        /// 顧客データを読み込み
        /// </summary>
        private async Task LoadCustomersAsync()
        {
            try
            {
                _view.SetLoading(true);
                var customers = await _repository.GetAllAsync();
                _view.ShowCustomers(customers);
            }
            catch (Exception ex)
            {
                _view.ShowError("顧客データの読み込みに失敗しました。\nデータベース接続を確認してください。");
                // TODO: ログ出力
                Console.WriteLine($"Error loading customers: {ex}");
            }
            finally
            {
                _view.SetLoading(false);
            }
        }

        /// <summary>
        /// 顧客を削除
        /// </summary>
        /// <param name="customerId">削除する顧客のID</param>
        private async Task DeleteCustomerAsync(int customerId)
        {
            try
            {
                _view.SetLoading(true);
                bool success = await _repository.DeleteAsync(customerId);
                
                if (success)
                {
                    _view.ShowSuccess("顧客を削除しました。");
                    await LoadCustomersAsync(); // 一覧を再読み込み
                }
                else
                {
                    _view.ShowError("顧客の削除に失敗しました。");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("顧客の削除中にエラーが発生しました。");
                // TODO: ログ出力
                Console.WriteLine($"Error deleting customer: {ex}");
            }
            finally
            {
                _view.SetLoading(false);
            }
        }

        /// <summary>
        /// 顧客編集ダイアログを表示
        /// </summary>
        /// <param name="customer">編集する顧客（新規の場合はnull）</param>
        private void ShowCustomerEditDialog(Customer? customer)
        {
            // 顧客編集フォームを作成し、モーダルで表示
            var editForm = new Views.CustomerEditView();
            var editPresenter = new CustomerEditPresenter(editForm, _repository);

            // 編集モードの設定
            if (customer != null)
            {
                editForm.IsEditMode = true;
                editForm.SetCustomer(customer);
            }
            else
            {
                editForm.IsEditMode = false;
            }

            // モーダルダイアログとして表示
            var result = editForm.ShowDialog();
            
            // ダイアログが正常に閉じられた場合、一覧を更新
            if (result == DialogResult.OK)
            {
                _ = LoadCustomersAsync(); // 非同期で一覧を更新（fire and forget）
            }

            // リソースのクリーンアップ
            editForm.Dispose();
        }
    }
}