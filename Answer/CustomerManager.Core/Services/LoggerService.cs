using Microsoft.Extensions.Logging;
using CustomerManager.Core.Interfaces;

namespace CustomerManager.Core.Services
{
    /// <summary>
    /// ログ出力サービスの実装
    /// Microsoft.Extensions.LoggingとNLogを使用したログ管理
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// デバッグレベルのログを出力
        /// </summary>
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        /// <summary>
        /// 情報レベルのログを出力
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// 警告レベルのログを出力
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// エラーレベルのログを出力
        /// </summary>
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        /// <summary>
        /// 例外情報付きエラーログを出力
        /// </summary>
        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        /// <summary>
        /// 致命的エラーレベルのログを出力
        /// </summary>
        public void LogFatal(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        /// <summary>
        /// 例外情報付き致命的エラーログを出力
        /// </summary>
        public void LogFatal(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }

        /// <summary>
        /// 処理の開始をログ出力
        /// </summary>
        public void LogMethodStart(string methodName, params object[] parameters)
        {
            if (parameters?.Length > 0)
            {
                _logger.LogDebug("メソッド開始: {MethodName} パラメータ: {Parameters}", 
                    methodName, string.Join(", ", parameters));
            }
            else
            {
                _logger.LogDebug("メソッド開始: {MethodName}", methodName);
            }
        }

        /// <summary>
        /// 処理の終了をログ出力
        /// </summary>
        public void LogMethodEnd(string methodName, object? result = null)
        {
            if (result != null)
            {
                _logger.LogDebug("メソッド終了: {MethodName} 結果: {Result}", methodName, result);
            }
            else
            {
                _logger.LogDebug("メソッド終了: {MethodName}", methodName);
            }
        }

        /// <summary>
        /// データベース操作のログを出力
        /// </summary>
        public void LogDatabaseOperation(string operation, string tableName, int recordCount = 1)
        {
            _logger.LogInformation("データベース操作: {Operation} テーブル: {TableName} 件数: {RecordCount}", 
                operation, tableName, recordCount);
        }
    }
}