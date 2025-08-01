using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using CustomerManager.Data.DataAccess;
using CustomerManager.Core.Models;
using CustomerManager.Core.Interfaces;

namespace CustomerManager.Tests.Data
{
    /// <summary>
    /// CustomerRepositoryのユニットテスト
    /// データアクセス層の品質保証（InMemoryDatabaseを使用）
    /// </summary>
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly CustomerDbContext _context;
        private readonly ICustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            // InMemoryDatabaseを使用してテスト用のDbContextを作成
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CustomerDbContext(options);
            _repository = new CustomerRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #region GetAllAsync テスト

        [Fact]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WithData_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new[]
            {
                new Customer { Name = "田中太郎", Email = "tanaka@example.com" },
                new Customer { Name = "佐藤花子", Email = "sato@example.com" },
                new Customer { Name = "鈴木一郎", Email = "suzuki@example.com" }
            };

            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Select(c => c.Name).Should().Contain(new[] { "田中太郎", "佐藤花子", "鈴木一郎" });
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCustomersOrderedById()
        {
            // Arrange
            var customers = new[]
            {
                new Customer { Name = "Customer3", Email = "c3@example.com" },
                new Customer { Name = "Customer1", Email = "c1@example.com" },
                new Customer { Name = "Customer2", Email = "c2@example.com" }
            };

            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var customerList = result.ToList();
            customerList.Should().HaveCount(3);
            // IDでソートされていることを確認
            for (int i = 1; i < customerList.Count; i++)
            {
                customerList[i].Id.Should().BeGreaterThan(customerList[i - 1].Id);
            }
        }

        #endregion

        #region GetByIdAsync テスト

        [Fact]
        public async Task GetByIdAsync_ExistingId_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(customer.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("田中太郎");
            result.Email.Should().Be("tanaka@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddAsync テスト

        [Fact]
        public async Task AddAsync_ValidCustomer_ShouldAddAndReturnCustomer()
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
            var result = await _repository.AddAsync(customer);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("田中太郎");
            result.CreatedAt.Should().NotBeNull();
            result.CreatedAt.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            result.UpdatedAt.Should().NotBeNull();
            result.UpdatedAt.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

            // データベースに保存されていることを確認
            var savedCustomer = await _context.Customers.FindAsync(result.Id);
            savedCustomer.Should().NotBeNull();
            savedCustomer!.Name.Should().Be("田中太郎");
        }

        [Fact]
        public async Task AddAsync_NullCustomer_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null!));
        }

        #endregion

        #region UpdateAsync テスト

        [Fact]
        public async Task UpdateAsync_ExistingCustomer_ShouldUpdateAndReturnCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var originalCreatedAt = customer.CreatedAt;
            var originalUpdatedAt = customer.UpdatedAt;

            // 少し時間を空ける
            await Task.Delay(10);

            var updatedCustomer = new Customer
            {
                Id = customer.Id,
                Name = "田中太郎（更新）",
                Kana = "タナカタロウ",
                Email = "tanaka.updated@example.com",
                PhoneNumber = "090-9999-9999"
            };

            // Act
            var result = await _repository.UpdateAsync(updatedCustomer);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(customer.Id);
            result.Name.Should().Be("田中太郎（更新）");
            result.Email.Should().Be("tanaka.updated@example.com");
            result.CreatedAt.Should().Be(originalCreatedAt); // 作成日時は変更されない
            result.UpdatedAt.Should().NotBeNull();
            result.UpdatedAt.Should().BeAfter(originalUpdatedAt!.Value); // 更新日時は変更される

            // データベースで更新されていることを確認
            var savedCustomer = await _context.Customers.FindAsync(customer.Id);
            savedCustomer!.Name.Should().Be("田中太郎（更新）");
        }

        [Fact]
        public async Task UpdateAsync_NonExistingCustomer_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 999,
                Name = "存在しない顧客",
                Email = "nonexistent@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(customer));
        }

        [Fact]
        public async Task UpdateAsync_NullCustomer_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null!));
        }

        #endregion

        #region DeleteAsync テスト

        [Fact]
        public async Task DeleteAsync_ExistingCustomer_ShouldReturnTrueAndDeleteCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(customer.Id);

            // Assert
            result.Should().BeTrue();

            // データベースから削除されていることを確認
            var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_NonExistingCustomer_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region EmailExistsAsync テスト

        [Fact]
        public async Task EmailExistsAsync_ExistingEmail_ShouldReturnTrue()
        {
            // Arrange
            var customer = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.EmailExistsAsync("tanaka@example.com");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task EmailExistsAsync_NonExistingEmail_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.EmailExistsAsync("nonexistent@example.com");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task EmailExistsAsync_WithExcludeId_ShouldExcludeSpecifiedCustomer()
        {
            // Arrange
            var customer1 = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            var customer2 = new Customer { Name = "佐藤花子", Email = "sato@example.com" };
            _context.Customers.AddRange(customer1, customer2);
            await _context.SaveChangesAsync();

            // Act（自分自身を除外してチェック）
            var result = await _repository.EmailExistsAsync("tanaka@example.com", customer1.Id);

            // Assert
            result.Should().BeFalse(); // 自分自身は除外されるのでfalse
        }

        [Fact]
        public async Task EmailExistsAsync_WithExcludeId_DifferentCustomerWithSameEmail_ShouldReturnTrue()
        {
            // Arrange
            var customer1 = new Customer { Name = "田中太郎", Email = "tanaka@example.com" };
            var customer2 = new Customer { Name = "佐藤花子", Email = "tanaka@example.com" }; // 同じメール
            _context.Customers.AddRange(customer1, customer2);
            await _context.SaveChangesAsync();

            // Act（customer1を除外してチェック）
            var result = await _repository.EmailExistsAsync("tanaka@example.com", customer1.Id);

            // Assert
            result.Should().BeTrue(); // customer2が同じメールを持っているのでtrue
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task EmailExistsAsync_EmptyOrNullEmail_ShouldReturnFalse(string? email)
        {
            // Act
            var result = await _repository.EmailExistsAsync(email!);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}