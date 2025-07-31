using CustomerManager.Core.Models;

namespace CustomerManager.Core.Interfaces
{
    /// <summary>
    /// 顧客登録・編集画面のインターフェース
    /// UIとビジネスロジックの分離のため
    /// </summary>
    public interface ICustomerEditView
    {
        /// <summary>
        /// 編集モードかどうか
        /// </summary>
        bool IsEditMode { get; set; }

        /// <summary>
        /// 顧客データを画面に設定
        /// </summary>
        /// <param name="customer">設定する顧客データ</param>
        void SetCustomer(Customer customer);

        /// <summary>
        /// 画面から顧客データを取得
        /// </summary>
        /// <returns>入力された顧客データ</returns>
        Customer GetCustomer();

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
        /// フィールドのバリデーションエラーを表示
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        void ShowFieldError(string fieldName, string errorMessage);

        /// <summary>
        /// 全てのエラー表示をクリア
        /// </summary>
        void ClearErrors();

        /// <summary>
        /// ローディング状態を設定
        /// </summary>
        /// <param name="isLoading">ローディング中の場合true</param>
        void SetLoading(bool isLoading);

        /// <summary>
        /// ダイアログを閉じる
        /// </summary>
        void CloseDialog();

        // イベント定義
        event EventHandler SaveRequested;
        event EventHandler CancelRequested;
    }
}