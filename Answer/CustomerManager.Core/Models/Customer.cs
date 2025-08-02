using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Core.Models
{
    /// <summary>
    /// 顧客エンティティクラス
    /// データベースのcustomersテーブルに対応
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 顧客ID（主キー）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 氏名（必須）
        /// </summary>
        [Required(ErrorMessage = "氏名は必須です")]
        [StringLength(255, ErrorMessage = "氏名は255文字以内で入力してください")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// フリガナ
        /// </summary>
        [StringLength(255, ErrorMessage = "フリガナは255文字以内で入力してください")]
        public string? Kana { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [StringLength(20, ErrorMessage = "電話番号は20文字以内で入力してください")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス（必須・一意）
        /// </summary>
        [Required(ErrorMessage = "メールアドレスは必須です")]
        [EmailAddress(ErrorMessage = "正しいメールアドレス形式で入力してください")]
        [StringLength(255, ErrorMessage = "メールアドレスは255文字以内で入力してください")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}