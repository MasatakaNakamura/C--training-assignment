using Xunit;

namespace CustomerManager.Tests
{
    /// <summary>
    /// 基本テスト - テストランナーが正常に動作するかの確認用
    /// </summary>
    public class BasicTests
    {
        [Fact]
        public void BasicTest_ShouldPass()
        {
            // Arrange
            int expected = 4;

            // Act
            int actual = 2 + 2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 3, 5)]
        [InlineData(-1, 1, 0)]
        public void AdditionTest_ShouldReturnCorrectSum(int a, int b, int expected)
        {
            // Act
            int actual = a + b;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}