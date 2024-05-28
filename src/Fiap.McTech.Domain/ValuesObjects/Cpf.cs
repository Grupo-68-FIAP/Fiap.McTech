using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.Domain.ValuesObjects
{
    /// <summary>
    /// Represents a CPF (Cadastro de Pessoas Físicas), which stands for "Individual Taxpayer Registry" in English.
    /// </summary>
    public class Cpf : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cpf"/> class with the specified CPF document.
        /// </summary>
        /// <param name="document">The CPF document.</param>
        public Cpf(string document)
        {
            Document = document;
        }

        /// <summary>
        /// Gets or sets the CPF document.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Validates the CPF document.
        /// </summary>
        /// <returns>True if the CPF document is valid; otherwise, false.</returns>
        public override bool IsValid() => Document.IsValidCpf();

        /// <summary>
        /// Returns a string representation of the CPF.
        /// </summary>
        /// <returns>The string representation of the CPF if it is valid; otherwise, an empty string.</returns>
        public override string ToString() => Document + (IsValid() ? "" : "<invalid>");

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is string)
                return Equals(obj, Document);
            else if (obj is Cpf cpf)
                return cpf.Document == Document;
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Document);
    }
}
