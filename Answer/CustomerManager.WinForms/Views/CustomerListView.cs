using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;
using CustomerManager.Core.Constants;

namespace CustomerManager.WinForms.Views
{
    /// <summary>
    /// 顧客一覧画面（View）
    /// ICustomerListViewインターフェースの実装
    /// MVPパターンのV（View）部分
    /// </summary>
    public partial class CustomerListView : Form, ICustomerListView
    {
        private DataGridView dataGridViewCustomers = null!;
        private Button buttonAdd = null!;
        private Button buttonEdit = null!;
        private Button buttonDelete = null!;
        private Button buttonRefresh = null!;
        private Label labelStatus = null!;
        private ProgressBar progressBar = null!;

        public CustomerListView()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        #region ICustomerListView Implementation

        /// <summary>
        /// 顧客リストを表示
        /// </summary>
        /// <param name="customers">表示する顧客リスト</param>
        public void ShowCustomers(IEnumerable<Customer> customers)
        {
            try
            {
                if (InvokeRequired)
                {
                    // より安全なInvoke処理
                    BeginInvoke(new Action(() => ShowCustomers(customers)));
                    return;
                }

                if (IsDisposed || Disposing) return;

                dataGridViewCustomers.DataSource = customers.ToList();
                labelStatus.Text = string.Format(MessageConstants.Status.CustomerCount, customers.Count());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ShowCustomers エラー: {ex.Message}");
                // エラーが発生してもアプリケーションを継続
            }
        }

        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public void ShowError(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowError), message);
                return;
            }

            MessageBox.Show(message, MessageConstants.DialogTitle.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 成功メッセージを表示
        /// </summary>
        /// <param name="message">成功メッセージ</param>
        public void ShowSuccess(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowSuccess), message);
                return;
            }

            MessageBox.Show(message, MessageConstants.DialogTitle.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 削除確認ダイアログを表示
        /// </summary>
        /// <param name="customerName">削除対象の顧客名</param>
        /// <returns>削除を確認した場合true</returns>
        public bool ConfirmDelete(string customerName)
        {
            var result = MessageBox.Show(
                string.Format(MessageConstants.Confirmation.DeleteCustomer, customerName),
                MessageConstants.DialogTitle.DeleteConfirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// 選択されている顧客を取得
        /// </summary>
        /// <returns>選択されている顧客、選択されていない場合はnull</returns>
        public Customer? GetSelectedCustomer()
        {
            if (dataGridViewCustomers.CurrentRow?.DataBoundItem is Customer customer)
            {
                return customer;
            }
            return null;
        }

        /// <summary>
        /// ローディング状態を設定
        /// </summary>
        /// <param name="isLoading">ローディング中の場合true</param>
        public void SetLoading(bool isLoading)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => SetLoading(isLoading)));
                    return;
                }

                if (IsDisposed || Disposing) return;

                progressBar.Visible = isLoading;
                
                // ボタンの有効/無効を切り替え
                buttonAdd.Enabled = !isLoading;
                buttonEdit.Enabled = !isLoading;
                buttonDelete.Enabled = !isLoading;
                buttonRefresh.Enabled = !isLoading;
                
                if (isLoading)
                {
                    labelStatus.Text = MessageConstants.Status.Loading;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetLoading エラー: {ex.Message}");
                // エラーが発生してもアプリケーションを継続
            }
        }

        // イベント定義
        public event EventHandler? LoadRequested;
        public event EventHandler? AddNewRequested;
        public event EventHandler? EditRequested;
        public event EventHandler? DeleteRequested;
        public event EventHandler? RefreshRequested;

        #endregion

        /// <summary>
        /// イベントハンドラーの初期化
        /// </summary>
        private void InitializeEventHandlers()
        {
            // フォームの読み込み時
            Load += (sender, e) => LoadRequested?.Invoke(this, EventArgs.Empty);

            // ボタンクリック時
            buttonAdd.Click += (sender, e) => AddNewRequested?.Invoke(this, EventArgs.Empty);
            buttonEdit.Click += (sender, e) => EditRequested?.Invoke(this, EventArgs.Empty);
            buttonDelete.Click += (sender, e) => DeleteRequested?.Invoke(this, EventArgs.Empty);
            buttonRefresh.Click += (sender, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);

            // DataGridViewの行ダブルクリックで編集
            dataGridViewCustomers.CellDoubleClick += (sender, e) =>
            {
                if (e.RowIndex >= 0) // ヘッダー行以外
                {
                    EditRequested?.Invoke(this, EventArgs.Empty);
                }
            };

            // 選択行変更時にボタンの有効/無効を切り替え
            dataGridViewCustomers.SelectionChanged += (sender, e) =>
            {
                bool hasSelection = GetSelectedCustomer() != null;
                buttonEdit.Enabled = hasSelection && !progressBar.Visible;
                buttonDelete.Enabled = hasSelection && !progressBar.Visible;
            };
        }
    }
}