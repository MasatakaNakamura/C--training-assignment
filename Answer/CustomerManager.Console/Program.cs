using CustomerManager.Core.Constants;
using CustomerManager.Core.Models;
using CustomerManager.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CustomerManager.Console
{
    /// <summary>
    /// コンソール版顧客管理アプリケーション
    /// Docker環境でのWindows Forms代替
    /// </summary>
    internal class Program
    {
        private static CustomerRepository? _repository;
        private static bool _running = true;

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("=== 顧客管理システム (コンソール版) ===");
            System.Console.WriteLine();

            try
            {
                // 初期化
                await InitializeAsync();

                // メインループ
                while (_running)
                {
                    await ShowMainMenuAsync();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラーが発生しました: {ex.Message}");
                System.Console.WriteLine("詳細: " + ex.ToString());
            }

            System.Console.WriteLine("アプリケーションを終了します。");
        }

        /// <summary>
        /// アプリケーション初期化
        /// </summary>
        private static async Task InitializeAsync()
        {
            // 設定読み込み
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("接続文字列が設定されていません。");
            }

            // DbContext設定
            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            // Repository作成
            var dbContext = new CustomerDbContext(optionsBuilder.Options);
            _repository = new CustomerRepository(dbContext);

            // DB接続テスト
            System.Console.WriteLine("データベース接続を確認中...");
            await dbContext.Database.CanConnectAsync();
            System.Console.WriteLine("✅ データベース接続成功");
            System.Console.WriteLine();
        }

        /// <summary>
        /// メインメニュー表示
        /// </summary>
        private static async Task ShowMainMenuAsync()
        {
            System.Console.WriteLine("=== メニュー ===");
            System.Console.WriteLine("1. 顧客一覧表示");
            System.Console.WriteLine("2. 顧客登録");
            System.Console.WriteLine("3. 顧客編集");
            System.Console.WriteLine("4. 顧客削除");
            System.Console.WriteLine("5. 顧客検索");
            System.Console.WriteLine("0. 終了");
            System.Console.Write("選択してください (0-5): ");

            var input = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (input)
            {
                case "1":
                    await ShowCustomersAsync();
                    break;
                case "2":
                    await AddCustomerAsync();
                    break;
                case "3":
                    await EditCustomerAsync();
                    break;
                case "4":
                    await DeleteCustomerAsync();
                    break;
                case "5":
                    await SearchCustomersAsync();
                    break;
                case "0":
                    _running = false;
                    break;
                default:
                    System.Console.WriteLine("無効な選択です。");
                    break;
            }

            if (_running)
            {
                System.Console.WriteLine("\nEnterキーを押して続行...");
                System.Console.ReadLine();
                System.Console.Clear();
            }
        }

        /// <summary>
        /// 顧客一覧表示
        /// </summary>
        private static async Task ShowCustomersAsync()
        {
            try
            {
                System.Console.WriteLine("=== 顧客一覧 ===");
                var customers = await _repository!.GetAllAsync();

                if (!customers.Any())
                {
                    System.Console.WriteLine("登録された顧客がありません。");
                    return;
                }

                System.Console.WriteLine($"{"ID",-5} {"氏名",-20} {"メールアドレス",-30} {"電話番号",-15}");
                System.Console.WriteLine(new string('-', 70));

                foreach (var customer in customers)
                {
                    System.Console.WriteLine($"{customer.Id,-5} {customer.Name,-20} {customer.Email,-30} {customer.PhoneNumber ?? "未設定",-15}");
                }

                System.Console.WriteLine($"\n総件数: {customers.Count()}件");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 顧客登録
        /// </summary>
        private static async Task AddCustomerAsync()
        {
            try
            {
                System.Console.WriteLine("=== 顧客登録 ===");

                var customer = new Customer();

                System.Console.Write("氏名 (必須): ");
                customer.Name = System.Console.ReadLine() ?? "";

                System.Console.Write("フリガナ: ");
                customer.Kana = System.Console.ReadLine();

                System.Console.Write("電話番号: ");
                customer.PhoneNumber = System.Console.ReadLine();

                System.Console.Write("メールアドレス (必須): ");
                customer.Email = System.Console.ReadLine() ?? "";

                // 簡易バリデーション
                if (string.IsNullOrWhiteSpace(customer.Name))
                {
                    System.Console.WriteLine("エラー: 氏名は必須です。");
                    return;
                }

                if (string.IsNullOrWhiteSpace(customer.Email))
                {
                    System.Console.WriteLine("エラー: メールアドレスは必須です。");
                    return;
                }

                // 重複チェック
                if (await _repository!.EmailExistsAsync(customer.Email))
                {
                    System.Console.WriteLine(MessageConstants.Validation.EmailDuplicate);
                    return;
                }

                // 登録実行
                await _repository.AddAsync(customer);
                System.Console.WriteLine("✅ 顧客を登録しました。");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 顧客編集
        /// </summary>
        private static async Task EditCustomerAsync()
        {
            try
            {
                System.Console.WriteLine("=== 顧客編集 ===");
                System.Console.Write("編集する顧客のID: ");

                if (!int.TryParse(System.Console.ReadLine(), out int id))
                {
                    System.Console.WriteLine("無効なIDです。");
                    return;
                }

                var customer = await _repository!.GetByIdAsync(id);
                if (customer == null)
                {
                    System.Console.WriteLine("指定されたIDの顧客が見つかりません。");
                    return;
                }

                System.Console.WriteLine($"現在の情報: {customer.Name} ({customer.Email})");
                System.Console.WriteLine();

                System.Console.Write($"氏名 (現在: {customer.Name}): ");
                var newName = System.Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                    customer.Name = newName;

                System.Console.Write($"フリガナ (現在: {customer.Kana ?? "未設定"}): ");
                var newKana = System.Console.ReadLine();
                customer.Kana = string.IsNullOrWhiteSpace(newKana) ? customer.Kana : newKana;

                System.Console.Write($"電話番号 (現在: {customer.PhoneNumber ?? "未設定"}): ");
                var newPhone = System.Console.ReadLine();
                customer.PhoneNumber = string.IsNullOrWhiteSpace(newPhone) ? customer.PhoneNumber : newPhone;

                System.Console.Write($"メールアドレス (現在: {customer.Email}): ");
                var newEmail = System.Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newEmail) && newEmail != customer.Email)
                {
                    if (await _repository.EmailExistsAsync(newEmail, customer.Id))
                    {
                        System.Console.WriteLine(MessageConstants.Validation.EmailDuplicate);
                        return;
                    }
                    customer.Email = newEmail;
                }

                await _repository.UpdateAsync(customer);
                System.Console.WriteLine("✅ 顧客情報を更新しました。");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 顧客削除
        /// </summary>
        private static async Task DeleteCustomerAsync()
        {
            try
            {
                System.Console.WriteLine("=== 顧客削除 ===");
                System.Console.Write("削除する顧客のID: ");

                if (!int.TryParse(System.Console.ReadLine(), out int id))
                {
                    System.Console.WriteLine("無効なIDです。");
                    return;
                }

                var customer = await _repository!.GetByIdAsync(id);
                if (customer == null)
                {
                    System.Console.WriteLine("指定されたIDの顧客が見つかりません。");
                    return;
                }

                System.Console.WriteLine($"削除対象: {customer.Name} ({customer.Email})");
                System.Console.Write("本当に削除しますか？ (y/N): ");

                var confirm = System.Console.ReadLine();
                if (confirm?.ToLower() != "y")
                {
                    System.Console.WriteLine("削除をキャンセルしました。");
                    return;
                }

                if (await _repository.DeleteAsync(id))
                {
                    System.Console.WriteLine("✅ 顧客を削除しました。");
                }
                else
                {
                    System.Console.WriteLine("削除に失敗しました。");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 顧客検索
        /// </summary>
        private static async Task SearchCustomersAsync()
        {
            try
            {
                System.Console.WriteLine("=== 顧客検索 ===");
                System.Console.Write("検索キーワード (氏名またはメールアドレス): ");

                var keyword = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    System.Console.WriteLine("キーワードを入力してください。");
                    return;
                }

                var allCustomers = await _repository!.GetAllAsync();
                var results = allCustomers.Where(c => 
                    c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (c.Kana?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

                if (!results.Any())
                {
                    System.Console.WriteLine("該当する顧客が見つかりませんでした。");
                    return;
                }

                System.Console.WriteLine($"\n検索結果: {results.Count}件");
                System.Console.WriteLine($"{"ID",-5} {"氏名",-20} {"メールアドレス",-30} {"電話番号",-15}");
                System.Console.WriteLine(new string('-', 70));

                foreach (var customer in results)
                {
                    System.Console.WriteLine($"{customer.Id,-5} {customer.Name,-20} {customer.Email,-30} {customer.PhoneNumber ?? "未設定",-15}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"エラー: {ex.Message}");
            }
        }
    }
}