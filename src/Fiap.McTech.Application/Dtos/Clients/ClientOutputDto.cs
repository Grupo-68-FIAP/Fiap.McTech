using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using System;

namespace Fiap.McTech.Application.Dtos.Clients
{
    public class ClientOutputDto
    {

        public ClientOutputDto(Client client)
        {
            Id = client.Id;
            Name = client.Name;
            Cpf = client.Cpf.ToString();
            Email = client.Email.ToString();
        }

        public ClientOutputDto(Guid id, string name, string cpf, string email)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
    }
}