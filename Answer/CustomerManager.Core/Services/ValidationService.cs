using System.ComponentModel.DataAnnotations;
using CustomerManager.Core.Models;
using CustomerManager.Core.Constants;

namespace CustomerManager.Core.Services
{
    /// <summary>
    /// バリデーション結果
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 顧客データのバリデーションサービス
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// 顧客データのバリデーションを実行
        /// </summary>
        /// <param name="customer">検証する顧客データ</param>
        /// <returns>バリデーション結果</returns>
        public ValidationResult ValidateCustomer(Customer customer)
        {
            var result = new ValidationResult();
            var context = new ValidationContext(customer);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Data Annotationsによるバリデーション
            bool isValid = Validator.TryValidateObject(customer, context, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    if (validationResult.MemberNames.Any())
                    {
                        var fieldName = validationResult.MemberNames.First();
                        result.Errors[fieldName] = validationResult.ErrorMessage ?? MessageConstants.Validation.InputError;
                    }
                }
            }

            // カスタムバリデーション
            ValidatePhoneNumber(customer.PhoneNumber, result);
            ValidateEmailFormat(customer.Email, result);

            result.IsValid = !result.Errors.Any();
            return result;
        }

        /// <summary>
        /// 電話番号の形式をチェック（簡易版）
        /// </summary>
        /// <param name="phoneNumber">電話番号</param>
        /// <param name="result">バリデーション結果</param>
        private void ValidatePhoneNumber(string? phoneNumber, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return; // 電話番号は任意項目なので空でもOK

            // 数字、ハイフン、括弧、スペースのみ許可する簡易チェック
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[\d\-\(\)\s]+$"))
            {
                result.Errors[FieldConstants.Customer.PhoneNumber] = MessageConstants.Validation.PhoneNumberFormat;
            }
        }

        /// <summary>
        /// メールアドレスの形式をより厳密にチェック
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <param name="result">バリデーション結果</param>
        private void ValidateEmailFormat(string email, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(email))
                return; // Data Annotationsで Required チェック済み

            // より厳密なメールアドレス検証
            // 基本的なメールアドレス形式をチェック
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                result.Errors[FieldConstants.Customer.Email] = "正しいメールアドレス形式で入力してください";
            }
        }
    }
}