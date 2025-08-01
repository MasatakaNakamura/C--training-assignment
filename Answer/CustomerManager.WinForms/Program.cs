using CustomerManager.Data.DataAccess;
using CustomerManager.WinForms.Presenters;
using CustomerManager.WinForms.Views;
using CustomerManager.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CustomerManager.WinForms
{
    /// <summary>
    /// アプリケーションのエントリーポイント
    /// 依存性の注入とアプリケーションの初期化を行う
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメインエントリポイント
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Debug.WriteLine("アプリケーション開始");
            ApplicationConfiguration.Initialize();

            // 開発中のスレッドチェックを無効化（本番では有効にすること）
            Control.CheckForIllegalCrossThreadCalls = false;
            Debug.WriteLine("スレッドチェックを無効化");

            // グローバル例外ハンドラを設定
            SetupGlobalExceptionHandlers();

            try
            {
                Debug.WriteLine("設定ファイル読み込み開始");
                // 設定ファイルを読み込み
                var configuration = BuildConfiguration();
                Debug.WriteLine("設定ファイル読み込み完了");
                
                // データベース接続設定
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Debug.WriteLine($"接続文字列: {connectionString}");
                if (string.IsNullOrEmpty(connectionString))
                {
                    Debug.WriteLine("接続文字列が見つかりません");
                    MessageBox.Show(
                        MessageConstants.Configuration.ConnectionStringMissing,
                        MessageConstants.DialogTitle.ConfigurationError,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // DbContextの設定
                var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

                Debug.WriteLine("データベース接続テスト開始");
                // データベース接続テスト
                try
                {
                    await TestDatabaseConnection(optionsBuilder.Options);
                    Debug.WriteLine("データベース接続テスト完了");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"データベース接続テスト失敗: {ex.Message}");
                    MessageBox.Show($"データベース接続に失敗しました。\n\nエラー: {ex.Message}\n\nMySQLコンテナが起動していることを確認してください。",
                        "データベース接続エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // 接続エラーがあってもアプリケーションは続行
                }

                // Repositoryの作成
                var dbContext = new CustomerDbContext(optionsBuilder.Options);
                var customerRepository = new CustomerRepository(dbContext);
                Debug.WriteLine("Repository作成完了");

                // メインフォームとPresenterの作成
                Debug.WriteLine("フォーム作成開始");
                using var mainForm = new CustomerListView();
                using var presenter = new CustomerListPresenter(mainForm, customerRepository);
                Debug.WriteLine("フォーム作成完了");

                // アプリケーション実行（UIスレッドで同期実行）
                Debug.WriteLine("Presenter初期化開始");
                
                // フォーム表示後にPresenterを初期化
                mainForm.Load += async (sender, e) =>
                {
                    try
                    {
                        Debug.WriteLine("UIスレッドでPresenter初期化開始");
                        await presenter.InitializeAsync();
                        Debug.WriteLine("UIスレッドでPresenter初期化完了");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Presenter初期化エラー: {ex.Message}");
                        MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                
                Debug.WriteLine("アプリケーション実行開始");
                Application.Run(mainForm);
                Debug.WriteLine("アプリケーション終了");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(MessageConstants.Configuration.StartupError, ex.Message),
                    MessageConstants.DialogTitle.StartupError,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 設定ファイルを構築
        /// </summary>
        /// <returns>設定オブジェクト</returns>
        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// データベース接続をテスト
        /// </summary>
        /// <param name="options">データベースオプション</param>
        private static async Task TestDatabaseConnection(DbContextOptions<CustomerDbContext> options)
        {
            try
            {
                using var context = new CustomerDbContext(options);
                await context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                var message = string.Format(MessageConstants.Database.ConnectionFailed, ex.Message);

                MessageBox.Show(message, MessageConstants.DialogTitle.DatabaseConnectionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// グローバル例外ハンドラを設定
        /// 未捕捉例外の一元管理
        /// </summary>
        private static void SetupGlobalExceptionHandlers()
        {
            // UIスレッドでの未捕捉例外をハンドル
            Application.ThreadException += Application_ThreadException;

            // セカンダリスレッドでの未捕捉例外をハンドル
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UIスレッドでの未捕捉例外ハンドラ
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogException(e.Exception, "UIスレッド未捕捉例外");
            
            // スレッド間操作エラーは無視してアプリケーションを継続
            if (e.Exception is InvalidOperationException && 
                e.Exception.Message.Contains("スレッド間の操作"))
            {
                Debug.WriteLine("スレッド間操作エラーを無視して継続");
                return;
            }
            
            var message = $"予期しないエラーが発生しました。\n\n" +
                         $"エラー: {e.Exception.Message}\n\n" +
                         $"アプリケーションを続行しますか？";

            var result = MessageBox.Show(
                message,
                MessageConstants.DialogTitle.Error,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);

            if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// セカンダリスレッドでの未捕捉例外ハンドラ
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogException(ex, "アプリケーションドメイン未捕捉例外");
                
                MessageBox.Show(
                    $"致命的なエラーが発生しました。\nアプリケーションを終了します。\n\nエラー: {ex.Message}",
                    MessageConstants.DialogTitle.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 例外をログに記録
        /// TODO: 実際のプロダクションでは適切なロギングライブラリを使用
        /// </summary>
        private static void LogException(Exception exception, string context)
        {
            try
            {
                var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n" +
                               $"Exception: {exception.GetType().Name}\n" +
                               $"Message: {exception.Message}\n" +
                               $"StackTrace: {exception.StackTrace}\n" +
                               $"----------------------------------------\n";

                // コンソールに出力（開発時のデバッグ用）
                Console.WriteLine(logMessage);

                // 実際のプロダクションでは、ファイルやデータベースにログを記録
                // File.AppendAllTextAsync("error.log", logMessage);
            }
            catch
            {
                // ログ記録でエラーが発生した場合は何もしない
                // 無限ループを防ぐため
            }
        }
    }
}