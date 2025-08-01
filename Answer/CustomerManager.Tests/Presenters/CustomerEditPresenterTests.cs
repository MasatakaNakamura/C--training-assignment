using Xunit;
using FluentAssertions;
using Moq;
using CustomerManager.Core.Interfaces;
using CustomerManager.Core.Models;
using CustomerManager.WinForms.Presenters;
using CustomerManager.Core.Constants;

namespace CustomerManager.Tests.Presenters
{
    /// <summary>
    /// CustomerEditPresenterのユニットテスト
    /// プレゼンター層のビジネスロジックの品質保証
    /// </summary>
    public class CustomerEditPresenterTests : IDisposable
    {
        private readonly Mock<ICustomerEditView> _mockView;
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly CustomerEditPresenter _presenter;

        public CustomerEditPresenterTests()
        {
            _mockView = new Mock<ICustomerEditView>();
            _mockRepository = new Mock<ICustomerRepository>();
            _presenter = new CustomerEditPresenter(_mockView.Object, _mockRepository.Object);
        }

        public void Dispose()
        {
            _presenter.Dispose();
        }

        #region コンストラクタテスト

        [Fact]
        public void Constructor_ValidParameters_ShouldSubscribeToViewEvents()
        {
            // Arrange & Act
            using var presenter = new CustomerEditPresenter(_mockView.Object, _mockRepository.Object);

            // Assert
            _mockView.VerifyAdd(v => v.SaveRequested += It.IsAny<EventHandler>(), Times.Once);
            _mockView.VerifyAdd(v => v.CancelRequested += It.IsAny<EventHandler>(), Times.Once);
        }

        [Fact]
        public void Constructor_NullView_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new CustomerEditPresenter(null!, _mockRepository.Object));
        }

        [Fact]
        public void Constructor_NullRepository_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new CustomerEditPresenter(_mockView.Object, null!));
        }

        #endregion

        #region SetCustomer テスト

        [Fact]
        public void SetCustomer_ValidCustomer_ShouldCallViewSetCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                Name = "田中太郎",
                Email = "tanaka@example.com"
            };

            // Act
            _presenter.SetCustomer(customer);

            // Assert
            _mockView.Verify(v => v.SetCustomer(customer), Times.Once);
        }

        #endregion

        #region SaveCustomer テスト - 新規登録

        [Fact]
        public async Task SaveCustomer_NewValidCustomer_ShouldAddCustomerAndCloseDialog()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "tanaka@example.com"
            };

            _mockView.Setup(v => v.IsEditMode).Returns(false);
            _mockView.Setup(v => v.GetCustomer()).Returns(customer);
            _mockRepository.Setup(r => r.EmailExistsAsync(customer.Email, null))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                .ReturnsAsync(customer);

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100); // 非同期処理の完了を待つ

            // Assert
            _mockView.Verify(v => v.SetLoading(true), Times.Once);
            _mockView.Verify(v => v.ClearErrors(), Times.Once);
            _mockRepository.Verify(r => r.EmailExistsAsync(customer.Email, null), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
            _mockView.Verify(v => v.ShowSuccess(MessageConstants.Success.CustomerCreated), Times.Once);
            _mockView.Verify(v => v.CloseDialog(), Times.Once);
            _mockView.Verify(v => v.SetLoading(false), Times.Once);
        }

        [Fact]
        public async Task SaveCustomer_NewCustomerWithDuplicateEmail_ShouldShowError()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "existing@example.com"
            };

            _mockView.Setup(v => v.IsEditMode).Returns(false);
            _mockView.Setup(v => v.GetCustomer()).Returns(customer);
            _mockRepository.Setup(r => r.EmailExistsAsync(customer.Email, null))
                .ReturnsAsync(true); // 重複メール

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100);

            // Assert
            _mockView.Verify(v => v.ShowFieldError(FieldConstants.Customer.Email, 
                MessageConstants.Validation.EmailDuplicate), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
            _mockView.Verify(v => v.CloseDialog(), Times.Never);
        }

        #endregion

        #region SaveCustomer テスト - 更新

        [Fact]
        public async Task SaveCustomer_EditExistingCustomer_ShouldUpdateCustomerAndCloseDialog()
        {
            // Arrange
            var originalCustomer = new Customer
            {
                Id = 1,
                Name = "田中太郎",
                Email = "tanaka@example.com",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var updatedCustomer = new Customer
            {
                Name = "田中太郎（更新）",
                Email = "tanaka@example.com"
            };

            _presenter.SetCustomer(originalCustomer);
            _mockView.Setup(v => v.IsEditMode).Returns(true);
            _mockView.Setup(v => v.GetCustomer()).Returns(updatedCustomer);
            _mockRepository.Setup(r => r.EmailExistsAsync(updatedCustomer.Email, originalCustomer.Id))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(updatedCustomer);

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100);

            // Assert
            _mockRepository.Verify(r => r.EmailExistsAsync(updatedCustomer.Email, originalCustomer.Id), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Customer>(c => 
                c.Id == originalCustomer.Id && 
                c.CreatedAt == originalCustomer.CreatedAt)), Times.Once);
            _mockView.Verify(v => v.ShowSuccess(MessageConstants.Success.CustomerUpdated), Times.Once);
            _mockView.Verify(v => v.CloseDialog(), Times.Once);
        }

        [Fact]
        public async Task SaveCustomer_EditCustomerWithSameEmail_ShouldSucceed()
        {
            // Arrange（同じメールアドレスでの更新は許可される）
            var originalCustomer = new Customer
            {
                Id = 1,
                Name = "田中太郎",
                Email = "tanaka@example.com"
            };

            var updatedCustomer = new Customer
            {
                Name = "田中太郎（更新）",
                Email = "tanaka@example.com" // 同じメール
            };

            _presenter.SetCustomer(originalCustomer);
            _mockView.Setup(v => v.IsEditMode).Returns(true);
            _mockView.Setup(v => v.GetCustomer()).Returns(updatedCustomer);
            _mockRepository.Setup(r => r.EmailExistsAsync(updatedCustomer.Email, originalCustomer.Id))
                .ReturnsAsync(false); // 自分自身は除外されるのでfalse
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(updatedCustomer);

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Once);
            _mockView.Verify(v => v.ShowSuccess(MessageConstants.Success.CustomerUpdated), Times.Once);
        }

        #endregion

        #region SaveCustomer テスト - バリデーション

        [Fact]
        public async Task SaveCustomer_InvalidCustomer_ShouldShowValidationErrors()
        {
            // Arrange
            var invalidCustomer = new Customer
            {
                Name = "", // 空の名前
                Email = "invalid-email" // 無効なメール
            };

            _mockView.Setup(v => v.IsEditMode).Returns(false);
            _mockView.Setup(v => v.GetCustomer()).Returns(invalidCustomer);

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100);

            // Assert
            _mockView.Verify(v => v.ShowFieldError(FieldConstants.Customer.Name, It.IsAny<string>()), Times.Once);
            _mockView.Verify(v => v.ShowFieldError(FieldConstants.Customer.Email, It.IsAny<string>()), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
            _mockView.Verify(v => v.CloseDialog(), Times.Never);
        }

        #endregion

        #region SaveCustomer テスト - エラーハンドリング

        [Fact]
        public async Task SaveCustomer_RepositoryThrowsException_ShouldShowErrorAndStopLoading()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "田中太郎",
                Email = "tanaka@example.com"
            };

            _mockView.Setup(v => v.IsEditMode).Returns(false);
            _mockView.Setup(v => v.GetCustomer()).Returns(customer);
            _mockRepository.Setup(r => r.EmailExistsAsync(customer.Email, null))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception("データベースエラー"));

            // Act
            _mockView.Raise(v => v.SaveRequested += null, EventArgs.Empty);
            await Task.Delay(100);

            // Assert
            _mockView.Verify(v => v.ShowError(MessageConstants.Database.SaveFailed), Times.Once);
            _mockView.Verify(v => v.SetLoading(false), Times.Once);
            _mockView.Verify(v => v.CloseDialog(), Times.Never);
        }

        #endregion

        #region CancelRequested テスト

        [Fact]
        public void CancelRequested_ShouldCloseDialog()
        {
            // Act
            _mockView.Raise(v => v.CancelRequested += null, EventArgs.Empty);

            // Assert
            _mockView.Verify(v => v.CloseDialog(), Times.Once);
        }

        #endregion

        #region Dispose テスト

        [Fact]
        public void Dispose_ShouldUnsubscribeFromViewEvents()
        {
            // Act
            _presenter.Dispose();

            // Assert
            _mockView.VerifyRemove(v => v.SaveRequested -= It.IsAny<EventHandler>(), Times.Once);
            _mockView.VerifyRemove(v => v.CancelRequested -= It.IsAny<EventHandler>(), Times.Once);
        }

        #endregion
    }
}