using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.ValuesObjects;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Clients
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        Task<Client?> GetClientAsync(Cpf cpf);
        Task<Client?> GetClientAsync(Email cpf);
    }
}