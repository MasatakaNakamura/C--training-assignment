using CustomerManager.Data.DataAccess;
using CustomerManager.WinForms.Presenters;
using CustomerManager.WinForms.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            ApplicationConfiguration.Initialize();

            try
            {
                // 設定ファイルを読み込み
                var configuration = BuildConfiguration();
                
                // データベース接続設定
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show(
                        "データベース接続文字列が設定されていません。\nappsettings.json を確認してください。",
                        "設定エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // DbContextの設定
                var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                // データベース接続テスト
                await TestDatabaseConnection(optionsBuilder.Options);

                // Repositoryの作成
                var dbContext = new CustomerDbContext(optionsBuilder.Options);
                var customerRepository = new CustomerRepository(dbContext);

                // メインフォームとPresenterの作成
                var mainForm = new CustomerListView();
                var presenter = new CustomerListPresenter(mainForm, customerRepository);

                // アプリケーション実行
                await presenter.InitializeAsync();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"アプリケーションの起動中にエラーが発生しました。\n\n{ex.Message}",
                    "起動エラー",
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
                var message = "データベースに接続できませんでした。以下を確認してください：\n\n" +
                             "1. MySQLサーバーが起動しているか\n" +
                             "2. 接続文字列が正しいか\n" +
                             "3. データベースとテーブルが作成されているか\n\n" +
                             $"詳細エラー: {ex.Message}";

                MessageBox.Show(message, "データベース接続エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}