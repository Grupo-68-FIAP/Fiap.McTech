using Fiap.McTech.Domain.ValuesObjects;
using System;

namespace Fiap.McTech.Application.ViewModels.Clients
{
	public class ClientInputDto
    {
        public ClientInputDto(string name, string cpf, string email)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}