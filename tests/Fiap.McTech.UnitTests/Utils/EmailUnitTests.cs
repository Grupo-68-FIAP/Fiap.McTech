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

		[Fact]
		public void TestInvalidEmail_NoAtSymbol()
		{
			// Arrange
			string invalidEmail = "testexample.com";

			// Act
			bool isValid = invalidEmail.IsValidEmail();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidEmail_NoDomain()
		{
			// Arrange
			string invalidEmail = "test@";

			// Act
			bool isValid = invalidEmail.IsValidEmail();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidEmail_NoUsername()
		{
			// Arrange
			string invalidEmail = "@example.com"; 

			// Act
			bool isValid = invalidEmail.IsValidEmail();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidEmail_NullOrEmpty()
		{
			// Arrange
			string nullOrEmptyEmail = null; 

			// Act
			bool isValidNullOrEmpty = nullOrEmptyEmail.IsValidEmail();

			// Assert
			Assert.False(isValidNullOrEmpty);
		}

		[Fact]
		public void TestInvalidEmail_WhiteSpace()
		{
			// Arrange
			string whiteSpaceEmail = "   ";

			// Act
			bool isValidWhiteSpace = whiteSpaceEmail.IsValidEmail();

			// Assert
			Assert.False(isValidWhiteSpace);
		}
	}
}