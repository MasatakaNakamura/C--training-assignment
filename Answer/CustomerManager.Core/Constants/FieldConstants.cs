namespace CustomerManager.Core.Constants
{
    /// <summary>
    /// フィールド名定数
    /// バリデーションエラー表示やプロパティ名参照で使用
    /// </summary>
    public static class FieldConstants
    {
        /// <summary>
        /// 顧客エンティティのフィールド名
        /// </summary>
        public static class Customer
        {
            public const string Id = nameof(Models.Customer.Id);
            public const string Name = nameof(Models.Customer.Name);
            public const string Kana = nameof(Models.Customer.Kana);
            public const string PhoneNumber = nameof(Models.Customer.PhoneNumber);
            public const string Email = nameof(Models.Customer.Email);
            public const string CreatedAt = nameof(Models.Customer.CreatedAt);
            public const string UpdatedAt = nameof(Models.Customer.UpdatedAt);
        }
    }
}