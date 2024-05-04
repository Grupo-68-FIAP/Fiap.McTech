using Fiap.McTech.Application.ViewModels.Clients;

namespace Fiap.McTech.Application.Interfaces.AppServices
{
	public interface IClientAppService
	{
		Task<ClientViewModel?> GetClientById(Guid id);
	}
}