using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.Domain.ValuesObjects
{
    /// <summary>
    /// Represents an email address.
    /// </summary>
    public class Email : ValueObject
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class with the specified email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        public Email(string email)
        {
            Address = email;
        }

        /// <summary>
        /// Validates if the email address is valid.
        /// </summary>
        /// <returns>True if the email address is valid, otherwise false.</returns>
        public override bool IsValid() => Address.IsValidEmail();

        /// <summary>
        /// Returns the string representation of the email address.
        /// </summary>
        /// <returns>The email address if it's valid, otherwise an empty string.</returns>
        public override string ToString() => Address + (IsValid() ? "" : "<invalid>");
    }
}
