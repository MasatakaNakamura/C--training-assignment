using Xunit;
using FluentAssertions;
using CustomerManager.Core.Services;
using CustomerManager.Core.Models;
using CustomerManager.Core.Constants;

namespace CustomerManager.Tests.Services
{
    /// <summary>
    /// ValidationServiceのユニットテスト
    /// 入力検証ロジックの品質保証
    /// </summary>
    public class ValidationServiceTests
    {
        private readonly ValidationService _validationService;

        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        #region 正常系テスト

        [Fact]
        public void ValidateCustomer_ValidCustomer_ShouldReturnValid()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Kana = "タナカタロウ",
                Email = "tanaka@example.com",
                PhoneNumber = "090-1234-5678"
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateCustomer_MinimalValidCustomer_ShouldReturnValid()
        {
            // Arrange（必須項目のみ）
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "tanaka@example.com"
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        #endregion

        #region 異常系テスト - 名前

        [Fact]
        public void ValidateCustomer_EmptyName_ShouldReturnInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "",
                Email = "test@example.com"
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Name);
        }

        [Fact]
        public void ValidateCustomer_NullName_ShouldReturnInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Name = null!,
                Email = "test@example.com"
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Name);
        }

        [Fact]
        public void ValidateCustomer_TooLongName_ShouldReturnInvalid()
        {
            // Arrange（256文字の名前）
            var longName = new string('あ', 256);
            var customer = new Customer
            {
                Name = longName,
                Email = "test@example.com"
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Name);
        }

        #endregion

        #region 異常系テスト - メールアドレス

        [Fact]
        public void ValidateCustomer_EmptyEmail_ShouldReturnInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = ""
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Email);
        }

        [Fact]
        public void ValidateCustomer_NullEmail_ShouldReturnInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = null!
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Email);
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        [InlineData("test.example.com")]
        [InlineData("test@@example.com")]
        [InlineData("test@.com")]
        [InlineData("test@example")]
        [InlineData("test@example.")]
        public void ValidateCustomer_InvalidEmailFormat_ShouldReturnInvalid(string invalidEmail)
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = invalidEmail
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.Email);
        }

        #endregion

        #region 異常系テスト - 電話番号

        [Theory]
        [InlineData("abcd-efgh-ijkl")]
        [InlineData("090-abcd-5678")]
        [InlineData("電話番号")]
        [InlineData("090@1234@5678")]
        public void ValidateCustomer_InvalidPhoneNumberFormat_ShouldReturnInvalid(string invalidPhoneNumber)
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "test@example.com",
                PhoneNumber = invalidPhoneNumber
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainKey(FieldConstants.Customer.PhoneNumber);
        }

        [Theory]
        [InlineData("090-1234-5678")]
        [InlineData("03-1234-5678")]
        [InlineData("0120-123-456")]
        [InlineData("090 1234 5678")]
        [InlineData("(090) 1234-5678")]
        [InlineData("09012345678")]
        public void ValidateCustomer_ValidPhoneNumberFormat_ShouldReturnValid(string validPhoneNumber)
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "test@example.com",
                PhoneNumber = validPhoneNumber
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateCustomer_NullPhoneNumber_ShouldReturnValid()
        {
            // Arrange（電話番号は任意項目）
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "test@example.com",
                PhoneNumber = null
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateCustomer_EmptyPhoneNumber_ShouldReturnValid()
        {
            // Arrange（電話番号は任意項目）
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "test@example.com",
                PhoneNumber = ""
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        #endregion

        #region 複合エラーテスト

        [Fact]
        public void ValidateCustomer_MultipleErrors_ShouldReturnAllErrors()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "", // 空の名前
                Email = "invalid-email", // 無効なメール
                PhoneNumber = "invalid-phone" // 無効な電話番号
            };

            // Act
            var result = _validationService.ValidateCustomer(customer);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().ContainKey(FieldConstants.Customer.Name);
            result.Errors.Should().ContainKey(FieldConstants.Customer.Email);
            result.Errors.Should().ContainKey(FieldConstants.Customer.PhoneNumber);
        }

        #endregion
    }
}