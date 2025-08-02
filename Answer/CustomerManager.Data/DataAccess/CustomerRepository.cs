using Microsoft.EntityFrameworkCore;
using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;
using System.Diagnostics;

namespace CustomerManager.Data.DataAccess
{
    /// <summary>
    /// 顧客データアクセスの実装
    /// ICustomerRepositoryインターフェースの具体的な実装
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        private readonly ILoggerService? _logger;

        public CustomerRepository(CustomerDbContext context, ILoggerService? logger = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        /// <summary>
        /// 全顧客を取得
        /// </summary>
        /// <returns>顧客リスト</returns>
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            _logger?.LogMethodStart(methodName);
            
            try
            {
                _logger?.LogDebug("データベースクエリ開始: 全顧客取得");
                
                // CancellationTokenSourceでタイムアウトを設定
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                
                var result = await _context.Customers
                    .OrderBy(c => c.Id)
                    .ToListAsync(cts.Token)
                    .ConfigureAwait(false); // UIスレッドのコンテキストを回避
                
                _logger?.LogDatabaseOperation("SELECT", "customers", result.Count);
                _logger?.LogMethodEnd(methodName, $"{result.Count}件取得");
                return result;
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogError(ex, "データベースクエリタイムアウト: {MethodName}", methodName);
                throw new TimeoutException("データベースへの接続がタイムアウトしました。");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "データベースクエリエラー: {MethodName}", methodName);
                throw;
            }
        }

        /// <summary>
        /// IDで顧客を取得
        /// </summary>
        /// <param name="id">顧客ID</param>
        /// <returns>顧客データ、見つからない場合はnull</returns>
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 顧客を追加
        /// </summary>
        /// <param name="customer">追加する顧客</param>
        /// <returns>追加された顧客（IDが設定される）</returns>
        public async Task<Customer> AddAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            const string methodName = nameof(AddAsync);
            _logger?.LogMethodStart(methodName, customer.Name, customer.Email);
            
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                
                _logger?.LogDatabaseOperation("INSERT", "customers");
                _logger?.LogInfo("顧客登録完了: ID={CustomerId}, Name={CustomerName}", customer.Id, customer.Name);
                _logger?.LogMethodEnd(methodName, customer.Id);
                return customer;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "顧客登録エラー: {MethodName}, Name={CustomerName}", methodName, customer.Name);
                throw;
            }
        }

        /// <summary>
        /// 顧客を更新
        /// </summary>
        /// <param name="customer">更新する顧客</param>
        /// <returns>更新された顧客</returns>
        public async Task<Customer> UpdateAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // 既存のエンティティを取得
            var existingCustomer = await _context.Customers.FindAsync(customer.Id).ConfigureAwait(false);
            if (existingCustomer == null)
                throw new InvalidOperationException($"ID {customer.Id} の顧客が見つかりません。");

            // 変更があったプロパティのみを更新
            existingCustomer.Name = customer.Name;
            existingCustomer.Kana = customer.Kana;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Email = customer.Email;
            // CreatedAtは更新しない（作成日時は不変）
            // UpdatedAtは自動で設定される

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return existingCustomer;
        }

        /// <summary>
        /// 顧客を削除
        /// </summary>
        /// <param name="id">削除する顧客のID</param>
        /// <returns>削除が成功したかどうか</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// メールアドレスが既に存在するかチェック
        /// </summary>
        /// <param name="email">チェックするメールアドレス</param>
        /// <param name="excludeId">除外する顧客ID（更新時に自分自身を除外するため）</param>
        /// <returns>存在する場合true</returns>
        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var query = _context.Customers.Where(c => c.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}