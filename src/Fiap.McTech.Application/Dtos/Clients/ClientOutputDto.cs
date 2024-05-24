using Fiap.McTech.Domain.Entities.Clients;

namespace Fiap.McTech.Application.Dtos.Clients
{
    /// <summary>
    /// Represents the output data for a client.
    /// </summary>
    public class ClientOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientOutputDto"/> class from a <see cref="Client"/> entity.
        /// </summary>
        /// <param name="client">The client entity.</param>
        public ClientOutputDto(Client client)
        {
            Id = client.Id;
            Name = client.Name;
            Cpf = client.Cpf.ToString();
            Email = client.Email.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientOutputDto"/> class.
        /// </summary>
        /// <param name="id">The ID of the client.</param>
        /// <param name="name">The name of the client.</param>
        /// <param name="cpf">The CPF of the client.</param>
        /// <param name="email">The email of the client.</param>
        public ClientOutputDto(Guid id, string name, string cpf, string email)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        /// <summary>
        /// Gets the ID of the client.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the CPF of the client.
        /// </summary>
        public string Cpf { get; private set; }

        /// <summary>
        /// Gets the email of the client.
        /// </summary>
        public string Email { get; private set; }
    }
}