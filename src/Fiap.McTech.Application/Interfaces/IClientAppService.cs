using Fiap.McTech.Application.ViewModels.Clients;

namespace Fiap.McTech.Application.Interfaces
{
    public interface IClientAppService
    {
        Task<ClientViewModel?> GetClientById(Guid id);
    }
}