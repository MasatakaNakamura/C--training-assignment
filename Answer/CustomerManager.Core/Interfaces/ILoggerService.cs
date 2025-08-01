namespace CustomerManager.Core.Interfaces
{
    /// <summary>
    /// ログ出力サービスのインターフェース
    /// アプリケーション全体のログ管理を統一化
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// デバッグレベルのログを出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// 情報レベルのログを出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// 警告レベルのログを出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// エラーレベルのログを出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// 例外情報付きエラーログを出力
        /// </summary>
        /// <param name="exception">例外オブジェクト</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogError(Exception exception, string message, params object[] args);

        /// <summary>
        /// 致命的エラーレベルのログを出力
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogFatal(string message, params object[] args);

        /// <summary>
        /// 例外情報付き致命的エラーログを出力
        /// </summary>
        /// <param name="exception">例外オブジェクト</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="args">メッセージのパラメータ</param>
        void LogFatal(Exception exception, string message, params object[] args);

        /// <summary>
        /// 処理の開始をログ出力
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <param name="parameters">パラメータ</param>
        void LogMethodStart(string methodName, params object[] parameters);

        /// <summary>
        /// 処理の終了をログ出力
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <param name="result">結果</param>
        void LogMethodEnd(string methodName, object? result = null);

        /// <summary>
        /// データベース操作のログを出力
        /// </summary>
        /// <param name="operation">操作名</param>
        /// <param name="tableName">テーブル名</param>
        /// <param name="recordCount">処理件数</param>
        void LogDatabaseOperation(string operation, string tableName, int recordCount = 1);
    }
}