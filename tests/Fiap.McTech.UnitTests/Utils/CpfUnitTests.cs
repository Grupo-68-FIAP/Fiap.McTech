using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.UnitTests.Utils
{
    public class CpfUnitTests
    {
        [Fact]
        public void TestValidCpf()
        {
            // Arrange
            string validCpf = "12345678909";

            // Act
            bool isValid = validCpf.IsValidCpf();

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("1234567890")]
        [InlineData("11111111111")]
        [InlineData("12345678901")]
        public void TestInvalidCpf_InvalidData(string invalidCpf)
        {
            // Act
            bool isValid = invalidCpf.IsValidCpf();

            // Assert
            Assert.False(isValid);
        }
    }
}
