using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Utils.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Clients
{
    /// <summary>
    /// Represents the input data for a client.
    /// </summary>
    public class ClientInputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInputDto"/> class.
        /// </summary>
        /// <param name="name">The name of the client.</param>
        /// <param name="cpf">The CPF of the client.</param>
        /// <param name="email">The email of the client.</param>
        public ClientInputDto(string name, string cpf, string email)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CPF of the client.
        /// </summary>
        [Required]
        [MaxLength(11), MinLength(11)]
        [CustomValidation(typeof(ClientInputDto), nameof(Validate))]
        public string Cpf { get; set; }

        /// <summary>
        /// Gets or sets the email of the client.
        /// </summary>
        [Required]
        [CustomValidation(typeof(ClientInputDto), nameof(Validate))]
        public string Email { get; set; }

        /// <summary>
        /// Converts the DTO to a domain entity.
        /// </summary>
        /// <returns>A <see cref="Client"/> instance.</returns>
        public Client ToClient()
        {
            return new Client(Name, new Domain.ValuesObjects.Cpf(Cpf), new Domain.ValuesObjects.Email(Email));
        }

        /// <summary>
        /// Validates the CPF or email.
        /// </summary>
        /// <param name="field">The field value.</param>
        /// <param name="context">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the field is valid or not.</returns>
        public static ValidationResult? Validate(string field, ValidationContext context)
        {
            if (string.IsNullOrEmpty(field)) return new ValidationResult($"Invalid {context.DisplayName}.");
            else if (context.DisplayName == nameof(Cpf) && field.IsValidCpf()) return ValidationResult.Success;
            else if (context.DisplayName == nameof(Email) && field.IsValidEmail()) return ValidationResult.Success;
            return new ValidationResult($"Invalid {context.DisplayName}.");
        }
    }
}
