using Fiap.McTech.Application.Dtos.Clients;

namespace Fiap.McTech.Application.Interfaces
{
    public interface IClientAppService
    {
        Task<ClientOutputDto?> GetClientByIdAsync(Guid id);
        Task<ClientOutputDto?> GetClientByCpfAsync(string cpf);
        Task<ClientOutputDto> CreateClientAsync(ClientInputDto id);
        Task<List<ClientOutputDto>> GetAllClientsAsync();
        Task<ClientOutputDto> UpdateClientAsync(Guid id, ClientInputDto dto);
        Task DeleteClientAsync(Guid id);
    }
}