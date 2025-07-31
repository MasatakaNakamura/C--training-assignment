using CustomerManager.Core.Models;

namespace CustomerManager.Core.Interfaces
{
    /// <summary>
    /// 顧客一覧画面のインターフェース
    /// UIとビジネスロジックの分離のため
    /// </summary>
    public interface ICustomerListView
    {
        /// <summary>
        /// 顧客リストを表示
        /// </summary>
        /// <param name="customers">表示する顧客リスト</param>
        void ShowCustomers(IEnumerable<Customer> customers);

        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        void ShowError(string message);

        /// <summary>
        /// 成功メッセージを表示
        /// </summary>
        /// <param name="message">成功メッセージ</param>
        void ShowSuccess(string message);

        /// <summary>
        /// 削除確認ダイアログを表示
        /// </summary>
        /// <param name="customerName">削除対象の顧客名</param>
        /// <returns>削除を確認した場合true</returns>
        bool ConfirmDelete(string customerName);

        /// <summary>
        /// 選択されている顧客を取得
        /// </summary>
        /// <returns>選択されている顧客、選択されていない場合はnull</returns>
        Customer? GetSelectedCustomer();

        /// <summary>
        /// ローディング状態を設定
        /// </summary>
        /// <param name="isLoading">ローディング中の場合true</param>
        void SetLoading(bool isLoading);

        // イベント定義
        event EventHandler LoadRequested;
        event EventHandler AddNewRequested;
        event EventHandler EditRequested;
        event EventHandler DeleteRequested;
        event EventHandler RefreshRequested;
    }
}