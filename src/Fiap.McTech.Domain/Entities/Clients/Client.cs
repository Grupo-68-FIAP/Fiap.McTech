using Fiap.McTech.Domain.ValuesObjects;

namespace Fiap.McTech.Domain.Entities.Clients
{
    /// <summary>
    /// Represents a client in the system.
    /// </summary>
    public class Client : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class with specified parameters.
        /// </summary>
        /// <param name="name">The name of the client.</param>
        /// <param name="cpf">The CPF (Brazilian individual taxpayer registry) of the client.</param>
        /// <param name="email">The email address of the client.</param>
        public Client(string name, Cpf cpf, Email email)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
        }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        public string Name { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CPF (Brazilian individual taxpayer registry) of the client.
        /// </summary>
        public Cpf Cpf { get; internal set; }

        /// <summary>
        /// Gets or sets the email address of the client.
        /// </summary>
        public Email Email { get; internal set; }

        /// <summary>
        /// Determines whether the client is valid.
        /// </summary>
        /// <returns>True if the client is valid; otherwise, false.</returns>
		public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name)
                && Cpf != null && Cpf.IsValid()
                && Email != null && Email.IsValid();
        }
    }
}
