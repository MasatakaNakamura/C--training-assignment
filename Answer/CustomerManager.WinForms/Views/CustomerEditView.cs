using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;

namespace CustomerManager.WinForms.Views
{
    /// <summary>
    /// 顧客登録・編集画面（View）
    /// ICustomerEditViewインターフェースの実装
    /// MVPパターンのV（View）部分
    /// </summary>
    public partial class CustomerEditView : Form, ICustomerEditView
    {
        private TextBox textBoxName;
        private TextBox textBoxKana;
        private TextBox textBoxPhoneNumber;
        private TextBox textBoxEmail;
        private Button buttonSave;
        private Button buttonCancel;
        private Label labelName;
        private Label labelKana;
        private Label labelPhoneNumber;
        private Label labelEmail;
        private Label labelNameError;
        private Label labelKanaError;
        private Label labelPhoneNumberError;
        private Label labelEmailError;
        private Label labelGeneralError;
        private ProgressBar progressBar;

        public CustomerEditView()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        #region ICustomerEditView Implementation

        /// <summary>
        /// 編集モードかどうか
        /// </summary>
        public bool IsEditMode { get; set; } = false;

        /// <summary>
        /// 顧客データを画面に設定
        /// </summary>
        /// <param name="customer">設定する顧客データ</param>
        public void SetCustomer(Customer customer)
        {
            if (customer == null) return;

            textBoxName.Text = customer.Name;
            textBoxKana.Text = customer.Kana ?? string.Empty;
            textBoxPhoneNumber.Text = customer.PhoneNumber ?? string.Empty;
            textBoxEmail.Text = customer.Email;

            // タイトルを編集モードに変更
            this.Text = $"顧客編集 - {customer.Name}";
            buttonSave.Text = "更新(&U)";
        }

        /// <summary>
        /// 画面から顧客データを取得
        /// </summary>
        /// <returns>入力された顧客データ</returns>
        public Customer GetCustomer()
        {
            return new Customer
            {
                Name = textBoxName.Text.Trim(),
                Kana = string.IsNullOrWhiteSpace(textBoxKana.Text) ? null : textBoxKana.Text.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text) ? null : textBoxPhoneNumber.Text.Trim(),
                Email = textBoxEmail.Text.Trim()
            };
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

            labelGeneralError.Text = message;
            labelGeneralError.Visible = true;
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
        /// フィールドのバリデーションエラーを表示
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public void ShowFieldError(string fieldName, string errorMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(ShowFieldError), fieldName, errorMessage);
                return;
            }

            switch (fieldName)
            {
                case "Name":
                    labelNameError.Text = errorMessage;
                    labelNameError.Visible = true;
                    break;
                case "Kana":
                    labelKanaError.Text = errorMessage;
                    labelKanaError.Visible = true;
                    break;
                case "PhoneNumber":
                    labelPhoneNumberError.Text = errorMessage;
                    labelPhoneNumberError.Visible = true;
                    break;
                case "Email":
                    labelEmailError.Text = errorMessage;
                    labelEmailError.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// 全てのエラー表示をクリア
        /// </summary>
        public void ClearErrors()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearErrors));
                return;
            }

            labelNameError.Visible = false;
            labelKanaError.Visible = false;
            labelPhoneNumberError.Visible = false;
            labelEmailError.Visible = false;
            labelGeneralError.Visible = false;
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
            buttonSave.Enabled = !isLoading;
            buttonCancel.Enabled = !isLoading;

            // 入力フィールドの有効/無効を切り替え
            textBoxName.Enabled = !isLoading;
            textBoxKana.Enabled = !isLoading;
            textBoxPhoneNumber.Enabled = !isLoading;
            textBoxEmail.Enabled = !isLoading;
        }

        /// <summary>
        /// ダイアログを閉じる
        /// </summary>
        public void CloseDialog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(CloseDialog));
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // イベント定義
        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        #endregion

        /// <summary>
        /// イベントハンドラーの初期化
        /// </summary>
        private void InitializeEventHandlers()
        {
            // ボタンクリック時
            buttonSave.Click += (sender, e) => SaveRequested?.Invoke(this, EventArgs.Empty);
            buttonCancel.Click += (sender, e) => CancelRequested?.Invoke(this, EventArgs.Empty);

            // フォーム読み込み時
            Load += CustomerEditView_Load;

            // テキストボックスのフォーカス時にエラーをクリア
            textBoxName.Enter += (sender, e) => labelNameError.Visible = false;
            textBoxKana.Enter += (sender, e) => labelKanaError.Visible = false;
            textBoxPhoneNumber.Enter += (sender, e) => labelPhoneNumberError.Visible = false;
            textBoxEmail.Enter += (sender, e) => labelEmailError.Visible = false;

            // Enterキーで保存
            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter && e.Control)
                {
                    SaveRequested?.Invoke(this, EventArgs.Empty);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    CancelRequested?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        /// <summary>
        /// フォーム読み込み時の処理
        /// </summary>
        private void CustomerEditView_Load(object? sender, EventArgs e)
        {
            if (!IsEditMode)
            {
                this.Text = "顧客登録";
                buttonSave.Text = "登録(&S)";
            }

            // 氏名フィールドにフォーカスを設定
            textBoxName.Focus();
        }
    }
}