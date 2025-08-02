namespace CustomerManager.Core.Constants
{
    /// <summary>
    /// アプリケーション内で使用するメッセージ定数
    /// マジックストリングの排除と多言語対応の準備
    /// </summary>
    public static class MessageConstants
    {
        #region エラーメッセージ

        /// <summary>
        /// データベース関連エラーメッセージ
        /// </summary>
        public static class Database
        {
            public const string LoadFailed = "顧客データの読み込みに失敗しました。\nデータベース接続を確認してください。";
            public const string SaveFailed = "顧客情報の保存中にエラーが発生しました。\n再度お試しください。";
            public const string DeleteFailed = "顧客の削除中にエラーが発生しました。";
            public const string ConnectionFailed = "データベースに接続できませんでした。以下を確認してください：\n\n" +
                                                 "1. MySQLサーバーが起動しているか\n" +
                                                 "2. 接続文字列が正しいか\n" +
                                                 "3. データベースとテーブルが作成されているか\n\n" +
                                                 "詳細エラー: {0}";
        }

        /// <summary>
        /// バリデーション関連エラーメッセージ
        /// </summary>
        public static class Validation
        {
            public const string EmailDuplicate = "このメールアドレスは既に使用されています。";
            public const string SelectCustomerForEdit = "編集する顧客を選択してください。";
            public const string SelectCustomerForDelete = "削除する顧客を選択してください。";
            public const string InputError = "入力エラーがあります";
            public const string PhoneNumberFormat = "電話番号は数字、ハイフン、括弧、スペースのみ使用できます";
        }

        /// <summary>
        /// 設定関連エラーメッセージ  
        /// </summary>
        public static class Configuration
        {
            public const string ConnectionStringMissing = "データベース接続文字列が設定されていません。\nappsettings.json を確認してください。";
            public const string StartupError = "アプリケーションの起動中にエラーが発生しました。\n\n{0}";
        }

        #endregion

        #region 成功メッセージ

        /// <summary>
        /// CRUD操作成功メッセージ
        /// </summary>
        public static class Success
        {
            public const string CustomerCreated = "顧客を登録しました。";
            public const string CustomerUpdated = "顧客情報を更新しました。";
            public const string CustomerDeleted = "顧客を削除しました。";
        }

        #endregion

        #region 確認メッセージ

        /// <summary>
        /// 確認ダイアログメッセージ
        /// </summary>
        public static class Confirmation
        {
            public const string DeleteCustomer = "顧客「{0}」を削除してもよろしいですか？\nこの操作は取り消せません。";
        }

        #endregion

        #region ステータスメッセージ

        /// <summary>
        /// ステータス表示メッセージ
        /// </summary>
        public static class Status
        {
            public const string Loading = "読み込み中...";
            public const string CustomerCount = "顧客数: {0}件";
            public const string DatabaseSetupComplete = "データベースのセットアップが完了しました！";
        }

        #endregion

        #region ダイアログタイトル

        /// <summary>
        /// ダイアログタイトル
        /// </summary>
        public static class DialogTitle
        {
            public const string Error = "エラー";
            public const string Success = "完了";
            public const string Warning = "警告";
            public const string DeleteConfirmation = "削除確認";
            public const string DatabaseConnectionError = "データベース接続エラー";
            public const string ConfigurationError = "設定エラー";
            public const string StartupError = "起動エラー";
        }

        #endregion
    }
}