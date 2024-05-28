namespace Fiap.McTech.Domain.ValuesObjects
{
    public class CpfUnitTests
    {
        const string STRING_VALID_CPF_1 = "49245877027";
        const string STRING_VALID_CPF_2 = "07838171008";
        readonly Cpf validCpf1 = new(STRING_VALID_CPF_1);
        readonly Cpf validCpf2 = new(STRING_VALID_CPF_2);
        readonly Cpf invalidCpf = new("00011122233");

        [Fact]
        public void IsValid_Return_True()
        {
            // Act
            var result = validCpf1.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ToString_Return_ValidValue()
        {
            // Act
            var result = validCpf1.ToString();

            // Assert
            Assert.Contains(STRING_VALID_CPF_1, result);
        }

        [Fact]
        public void ToString_Return_InvalidValue()
        {
            // Act
            var result = invalidCpf.ToString();

            // Assert
            Assert.Contains("<invalid>", result);
        }

        [Fact]
        public void Equals_Return_CheckValues()
        {
            // Arrange
            var newForTest = new Cpf(STRING_VALID_CPF_1);
            var objFifferent = new Email("invalid-email");

            // Act
            var test1 = validCpf1.Equals(validCpf2);
            var test2 = validCpf1.Equals(STRING_VALID_CPF_1);
            var test3 = validCpf1.Equals(newForTest);
            var test4 = validCpf1.Equals("00011122233");
            var test5 = validCpf1.Equals(objFifferent);

            // Assert
            Assert.False(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.False(test4);
            Assert.False(test5);
        }
        [Fact]
        public void GetHashCode_Returns_SameHash_ForEqualObjects()
        {
            // Arrange
            var obj1 = new Cpf(STRING_VALID_CPF_1);
            var obj2 = new Cpf(STRING_VALID_CPF_1);

            // Act
            var hash1 = obj1.GetHashCode();
            var hash2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_Returns_DifferentHash_ForDifferentObjects()
        {
            // Arrange
            var obj1 = new Cpf(STRING_VALID_CPF_1);
            var obj2 = new Cpf(STRING_VALID_CPF_2);

            // Act
            var hash1 = obj1.GetHashCode();
            var hash2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
