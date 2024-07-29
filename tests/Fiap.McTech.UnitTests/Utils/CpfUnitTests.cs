using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.UnitTests.Utils
{
    public class CpfUnitTests
    {
        [Theory]
        [InlineData("12345678909")]
        [InlineData("39349474093")]
        [InlineData("52884751050")]
        public void TestValidCpf(string validCpf)
        {
            // Arrange & Act
            bool isValid = validCpf.IsValidCpf();

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("1234567890")]
        [InlineData("11111111111")]
        [InlineData("12345678901")]
        [InlineData("1234*901")]
        public void TestInvalidCpf_InvalidData(string invalidCpf)
        {
            // Act
            bool isValid = invalidCpf.IsValidCpf();

            // Assert
            Assert.False(isValid);
        }
    }
}
