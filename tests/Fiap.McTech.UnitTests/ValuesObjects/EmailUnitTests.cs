namespace Fiap.McTech.Domain.ValuesObjects
{
    public class EmailUnitTests
    {
        const string STRING_VALID_EMAIL_1 = "test1@test.com";
        const string STRING_VALID_EMAIL_2 = "test2@test.com";
        readonly Email validEmail1 = new(STRING_VALID_EMAIL_1);
        readonly Email validEmail2 = new(STRING_VALID_EMAIL_2);
        readonly Email invalidEmail = new("invalid-email");

        [Fact]
        public void IsValid_Return_True()
        {
            // Act
            var result = !validEmail1.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ToString_Return_ValidValue()
        {
            // Act
            var result = validEmail1.ToString();

            // Assert
            Assert.Contains(STRING_VALID_EMAIL_1, result);
        }

        [Fact]
        public void ToString_Return_InvalidValue()
        {
            // Act
            var result = invalidEmail.ToString();

            // Assert
            Assert.Contains("<invalid>", result);
        }

        [Fact]
        public void Equals_Return_CheckValues()
        {
            // Arrange
            var newForTest = new Email(STRING_VALID_EMAIL_1);
            var objFifferent = new Cpf("00011122233");

            // Act
            var test1 = validEmail1.Equals(validEmail2);
            var test2 = validEmail1.Equals(STRING_VALID_EMAIL_1);
            var test3 = validEmail1.Equals(newForTest);
            var test4 = validEmail1.Equals("00011122233");
            var test5 = validEmail1.Equals(objFifferent);

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
            var obj1 = new Email(STRING_VALID_EMAIL_1);
            var obj2 = new Email(STRING_VALID_EMAIL_1);

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
            var obj1 = new Email(STRING_VALID_EMAIL_1);
            var obj2 = new Email(STRING_VALID_EMAIL_2);

            // Act
            var hash1 = obj1.GetHashCode();
            var hash2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
