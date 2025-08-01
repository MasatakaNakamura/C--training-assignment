using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;
using CustomerManager.Core.Constants;
using System.Diagnostics;

namespace CustomerManager.WinForms.Presenters
{
    /// <summary>
    /// 顧客一覧画面のPresenter
    /// ViewとModelの仲介役
    /// MVPパターンのP（Presenter）部分
    /// </summary>
    public class CustomerListPresenter : IDisposable
    {
        private ICustomerListView? _view;
        private readonly ICustomerRepository _repository;
        private bool _disposed = false;

        public CustomerListPresenter(ICustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// ViewをPresenterにアタッチ
        /// </summary>
        public void AttachView(ICustomerListView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

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
            if (_view == null)
                throw new InvalidOperationException("View is not attached. Call AttachView first.");

            Debug.WriteLine("Presenter: LoadCustomersAsync開始");
            await LoadCustomersAsync();
            Debug.WriteLine("Presenter: LoadCustomersAsync完了");
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
            if (_view == null) return;
            ShowCustomerEditDialog(null);
        }

        /// <summary>
        /// 編集要求時の処理
        /// </summary>
        private void OnEditRequested(object? sender, EventArgs e)
        {
            if (_view == null) return;
            
            var selectedCustomer = _view.GetSelectedCustomer();
            if (selectedCustomer == null)
            {
                _view.ShowError(MessageConstants.Validation.SelectCustomerForEdit);
                return;
            }

            ShowCustomerEditDialog(selectedCustomer);
        }

        /// <summary>
        /// 削除要求時の処理
        /// </summary>
        private async void OnDeleteRequested(object? sender, EventArgs e)
        {
            if (_view == null) return;
            
            var selectedCustomer = _view.GetSelectedCustomer();
            if (selectedCustomer == null)
            {
                _view.ShowError(MessageConstants.Validation.SelectCustomerForDelete);
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
            if (_view == null) return;
            
            try
            {
                Debug.WriteLine("Presenter: SetLoading(true)");
                _view.SetLoading(true);
                Debug.WriteLine("Presenter: Repository.GetAllAsync開始");
                var customers = await _repository.GetAllAsync();
                Debug.WriteLine($"Presenter: Repository.GetAllAsync完了 - {customers.Count()}件取得");
                _view.ShowCustomers(customers);
                Debug.WriteLine("Presenter: ShowCustomers完了");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Presenter: エラー発生 - {ex.Message}");
                _view.ShowError(MessageConstants.Database.LoadFailed);
                // TODO: ログ出力
                Console.WriteLine($"Error loading customers: {ex}");
            }
            finally
            {
                Debug.WriteLine("Presenter: SetLoading(false)");
                _view.SetLoading(false);
            }
        }

        /// <summary>
        /// 顧客を削除
        /// </summary>
        /// <param name="customerId">削除する顧客のID</param>
        private async Task DeleteCustomerAsync(int customerId)
        {
            if (_view == null) return;
            
            try
            {
                _view.SetLoading(true);
                bool success = await _repository.DeleteAsync(customerId);
                
                if (success)
                {
                    _view.ShowSuccess(MessageConstants.Success.CustomerDeleted);
                    await LoadCustomersAsync(); // 一覧を再読み込み
                }
                else
                {
                    _view.ShowError(MessageConstants.Database.DeleteFailed);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(MessageConstants.Database.DeleteFailed);
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
            using var editForm = new Views.CustomerEditView();
            using var editPresenter = new CustomerEditPresenter(editForm, _repository);

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

            // usingによりリソースは自動的にクリーンアップされる
        }

        #region IDisposable Implementation

        /// <summary>
        /// リソースを解放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放（継承可能版）
        /// </summary>
        /// <param name="disposing">マネージドリソースを解放するかどうか</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Viewのイベント購読を解除
                    if (_view != null)
                    {
                        _view.LoadRequested -= OnLoadRequested;
                        _view.AddNewRequested -= OnAddNewRequested;
                        _view.EditRequested -= OnEditRequested;
                        _view.DeleteRequested -= OnDeleteRequested;
                        _view.RefreshRequested -= OnRefreshRequested;
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }
}