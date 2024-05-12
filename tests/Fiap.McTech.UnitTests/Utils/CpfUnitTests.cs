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

		[Fact]
		public void TestInvalidCpf_Length()
		{
			// Arrange
			string invalidLengthCpf = "1234567890"; 

			// Act
			bool isValid = invalidLengthCpf.IsValidCpf();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidCpf_RepeatedDigits()
		{
			// Arrange
			string invalidRepeatedDigitsCpf = "11111111111"; 

			// Act
			bool isValid = invalidRepeatedDigitsCpf.IsValidCpf();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidCpf_InvalidDigits()
		{
			// Arrange
			string invalidDigitsCpf = "12345678901"; 

			// Act
			bool isValid = invalidDigitsCpf.IsValidCpf();

			// Assert
			Assert.False(isValid);
		}

		[Fact]
		public void TestInvalidCpf_NullOrEmpty()
		{
			// Arrange
			string nullOrEmptyCpf = null; // CPF nulo

			// Act
			bool isValidNullOrEmpty = nullOrEmptyCpf.IsValidCpf();

			// Assert
			Assert.False(isValidNullOrEmpty);
		}
	}
}