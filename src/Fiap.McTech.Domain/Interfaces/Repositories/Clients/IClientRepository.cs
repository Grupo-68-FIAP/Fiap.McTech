using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.ValuesObjects;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Clients
{
    /// <summary>
    /// Represents a repository interface for CRUD operations with clients in the McTech domain.
    /// </summary>
    public interface IClientRepository : IRepositoryBase<Client>
    {
        /// <summary>
        /// Asynchronously retrieves a client by their CPF (Brazilian individual taxpayer registry) number.
        /// </summary>
        /// <param name="cpf">The CPF number of the client.</param>
        /// <returns>A task representing the asynchronous operation, containing the client with the specified CPF number, if found; otherwise, null.</returns>
        Task<Client?> GetClientAsync(Cpf cpf);

        /// <summary>
        /// Asynchronously retrieves a client by their email address.
        /// </summary>
        /// <param name="email">The email address of the client.</param>
        /// <returns>A task representing the asynchronous operation, containing the client with the specified email address, if found; otherwise, null.</returns>
        Task<Client?> GetClientAsync(Email email);
    }
}
