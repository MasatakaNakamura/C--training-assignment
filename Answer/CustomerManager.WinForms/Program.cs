using CustomerManager.Data.DataAccess;
using CustomerManager.WinForms.Presenters;
using CustomerManager.WinForms.Views;
using CustomerManager.Core.Constants;
using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
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

                // DIコンテナの設定
                var services = new ServiceCollection();
                
                // ログ設定
                services.AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.SetMinimumLevel(LogLevel.Debug);
                    builder.AddNLog();
                });
                
                // カスタムログサービスの登録
                services.AddSingleton<ILoggerService, LoggerService>();
                
                // DbContextの設定（各リクエストごとに新しいインスタンスを作成）
                services.AddDbContext<CustomerDbContext>(options =>
                    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));
                
                // Repositoryの登録（Scoped: 各操作ごとに新しいインスタンス）
                services.AddScoped<ICustomerRepository>(provider =>
                {
                    var context = provider.GetRequiredService<CustomerDbContext>();
                    var logger = provider.GetService<ILoggerService>();
                    return new CustomerRepository(context, logger);
                });
                
                // Presenterの登録（Transient: 毎回新しいインスタンス）
                services.AddTransient<CustomerListPresenter>(provider =>
                {
                    var repository = provider.GetRequiredService<ICustomerRepository>();
                    var logger = provider.GetService<ILoggerService>();
                    return new CustomerListPresenter(repository, logger);
                });
                
                var serviceProvider = services.BuildServiceProvider();
                Debug.WriteLine("DIコンテナ設定完了");

                Debug.WriteLine("データベース接続テスト開始");
                // データベース接続テスト
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var testContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                    await TestDatabaseConnection(testContext);
                    Debug.WriteLine("データベース接続テスト完了");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"データベース接続テスト失敗: {ex.Message}");
                    MessageBox.Show($"データベース接続に失敗しました。\n\nエラー: {ex.Message}\n\nMySQLコンテナが起動していることを確認してください。",
                        "データベース接続エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // 接続エラーがあってもアプリケーションは続行
                }

                Debug.WriteLine("フォーム作成開始");
                using var mainForm = new CustomerListView();
                Debug.WriteLine("フォーム作成完了");

                // Presenterのスコープをフォームのライフサイクルと合わせる
                IServiceScope? presenterScope = null;
                CustomerListPresenter? presenter = null;

                // フォーム表示後にPresenterを初期化
                mainForm.Load += async (sender, e) =>
                {
                    try
                    {
                        Debug.WriteLine("UIスレッドでPresenter初期化開始");
                        
                        // フォームのライフサイクルに合わせてスコープを作成
                        presenterScope = serviceProvider.CreateScope();
                        presenter = presenterScope.ServiceProvider.GetRequiredService<CustomerListPresenter>();
                        presenter.AttachView(mainForm);
                        
                        await presenter.InitializeAsync();
                        Debug.WriteLine("UIスレッドでPresenter初期化完了");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Presenter初期化エラー: {ex.Message}");
                        MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                // フォーム終了時にPresenterとスコープをクリーンアップ
                mainForm.FormClosed += (sender, e) =>
                {
                    try
                    {
                        Debug.WriteLine("Presenterクリーンアップ開始");
                        presenter?.Dispose();
                        presenterScope?.Dispose();
                        Debug.WriteLine("Presenterクリーンアップ完了");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Presenterクリーンアップエラー: {ex.Message}");
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
        private static async Task TestDatabaseConnection(CustomerDbContext context)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await context.Database.CanConnectAsync(cts.Token).ConfigureAwait(false);
                Debug.WriteLine("データベース接続テスト成功");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("データベース接続テストがタイムアウトしました。");
                throw new TimeoutException("データベースへの接続がタイムアウトしました。");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"データベース接続テストエラー: {ex.Message}");
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
                // NLogを使用したログ出力に変更
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error(exception, "{Context}: {ExceptionType} - {Message}", 
                    context, exception.GetType().Name, exception.Message);
            }
            catch
            {
                // ログ記録でエラーが発生した場合は何もしない
                // 無限ループを防ぐため
                var fallbackMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n" +
                                     $"Exception: {exception.GetType().Name}\n" +
                                     $"Message: {exception.Message}\n";
                Console.WriteLine(fallbackMessage);
            }
        }
    }
}