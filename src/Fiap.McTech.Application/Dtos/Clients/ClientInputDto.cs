using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Utils.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Clients
{
    public class ClientInputDto
    {
        public ClientInputDto(string name, string cpf, string email)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
        }
        [Required]
        public string Name { get; set; }
        [Required]
        [CustomValidation(typeof(ClientInputDto), nameof(Validate))]
        public string Cpf { get; set; }

        [Required]
        [CustomValidation(typeof(ClientInputDto), nameof(Validate))]
        public string Email { get; set; }

        public Client ToClient()
        {
            return new Client(Name, new Domain.ValuesObjects.Cpf(Cpf), new Domain.ValuesObjects.Email(Email));
        }

        public static ValidationResult? Validate(string field, ValidationContext context)
        {
            if (string.IsNullOrEmpty(field)) return new ValidationResult($"Invalid {context.DisplayName}.");
            else if (context.DisplayName == nameof(Cpf) && field.IsValidCpf()) return ValidationResult.Success;
            else if (context.DisplayName == nameof(Email) && field.IsValidEmail()) return ValidationResult.Success;
            return new ValidationResult($"Invalid {context.DisplayName}.");
        }
    }
}