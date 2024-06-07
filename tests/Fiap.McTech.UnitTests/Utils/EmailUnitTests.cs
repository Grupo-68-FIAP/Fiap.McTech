using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.UnitTests.Utils
{
    public class EmailTests
    {
        [Fact]
        public void TestValidEmail()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act
            bool isValid = validEmail.IsValidEmail();

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("@example.com")]
        [InlineData("test@")]
        [InlineData("   ")]
        [InlineData("testexample.com")]
        public void TestInvalidEmail_InvalidData(string invalidEmail)
        {
            // Act
            bool isValid = invalidEmail.IsValidEmail();

            // Assert
            Assert.False(isValid);
        }
    }
}
