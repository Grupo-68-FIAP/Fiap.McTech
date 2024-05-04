using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Clients
{
    public class ClientAppService : IClientAppService
	{
		private readonly IClientRepository _clientRepository;
		private readonly ILogger<ClientAppService> _logger;

		public ClientAppService(IClientRepository clientRepository, ILogger<ClientAppService> logger)
		{
			_clientRepository = clientRepository;
			_logger = logger;
		}

		public async Task<ClientViewModel?> GetClientById(Guid id)
		{
			try
			{
				var client = await _clientRepository.GetByIdAsync(id);
				if (client == null)
				{
					_logger.LogWarning($"Client with ID {id} not found");
					return null;
				}

				//TODO - ADD AUTO MAPPER
				return new ClientViewModel();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while fetching the client with ID {id}: {ex.Message}");
				throw;
			}
		}
	}
}