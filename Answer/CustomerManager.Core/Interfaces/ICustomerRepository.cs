using CustomerManager.Core.Models;

namespace CustomerManager.Core.Interfaces
{
    /// <summary>
    /// 顧客データアクセスのインターフェース
    /// データアクセス層への依存を抽象化
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// 全顧客を取得
        /// </summary>
        /// <returns>顧客リスト</returns>
        Task<IEnumerable<Customer>> GetAllAsync();

        /// <summary>
        /// IDで顧客を取得
        /// </summary>
        /// <param name="id">顧客ID</param>
        /// <returns>顧客データ、見つからない場合はnull</returns>
        Task<Customer?> GetByIdAsync(int id);

        /// <summary>
        /// 顧客を追加
        /// </summary>
        /// <param name="customer">追加する顧客</param>
        /// <returns>追加された顧客（IDが設定される）</returns>
        Task<Customer> AddAsync(Customer customer);

        /// <summary>
        /// 顧客を更新
        /// </summary>
        /// <param name="customer">更新する顧客</param>
        /// <returns>更新された顧客</returns>
        Task<Customer> UpdateAsync(Customer customer);

        /// <summary>
        /// 顧客を削除
        /// </summary>
        /// <param name="id">削除する顧客のID</param>
        /// <returns>削除が成功したかどうか</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// メールアドレスが既に存在するかチェック
        /// </summary>
        /// <param name="email">チェックするメールアドレス</param>
        /// <param name="excludeId">除外する顧客ID（更新時に自分自身を除外するため）</param>
        /// <returns>存在する場合true</returns>
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}