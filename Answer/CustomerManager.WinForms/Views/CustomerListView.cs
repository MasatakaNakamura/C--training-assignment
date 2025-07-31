using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;

namespace CustomerManager.WinForms.Views
{
    /// <summary>
    /// 顧客一覧画面（View）
    /// ICustomerListViewインターフェースの実装
    /// MVPパターンのV（View）部分
    /// </summary>
    public partial class CustomerListView : Form, ICustomerListView
    {
        private DataGridView dataGridViewCustomers;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonRefresh;
        private Label labelStatus;
        private ProgressBar progressBar;

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
            if (InvokeRequired)
            {
                Invoke(new Action<IEnumerable<Customer>>(ShowCustomers), customers);
                return;
            }

            dataGridViewCustomers.DataSource = customers.ToList();
            labelStatus.Text = $"顧客数: {customers.Count()}件";
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

            MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            MessageBox.Show(message, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 削除確認ダイアログを表示
        /// </summary>
        /// <param name="customerName">削除対象の顧客名</param>
        /// <returns>削除を確認した場合true</returns>
        public bool ConfirmDelete(string customerName)
        {
            var result = MessageBox.Show(
                $"顧客「{customerName}」を削除してもよろしいですか？\nこの操作は取り消せません。",
                "削除確認",
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
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetLoading), isLoading);
                return;
            }

            progressBar.Visible = isLoading;
            
            // ボタンの有効/無効を切り替え
            buttonAdd.Enabled = !isLoading;
            buttonEdit.Enabled = !isLoading;
            buttonDelete.Enabled = !isLoading;
            buttonRefresh.Enabled = !isLoading;
            
            if (isLoading)
            {
                labelStatus.Text = "読み込み中...";
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