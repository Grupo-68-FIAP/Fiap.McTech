using AutoMapper;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using Microsoft.Extensions.Logging;
using Fiap.McTech.Domain.Services;

namespace Fiap.McTech.Application.AppServices.Clients
{
    public class ClientAppService : IClientAppService
    {
        private readonly ClientService _clientService;
        private readonly ILogger<ClientAppService> _logger;
        private readonly IMapper _mapper;

        public ClientAppService(ClientService clientService, ILogger<ClientAppService> logger, IMapper mapper)
        {   
            _clientService = clientService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ClientOutputDto> CreateClientAsync(ClientInputDto clientDto)
        {
            try
            {
                _logger.LogInformation("Creating a new client.");

                var client = _mapper.Map<Client>(clientDto);

                var createdClient = await _clientService.CreateAsync(client);

                _logger.LogInformation("Client created successfully with ID {ClientId}.", createdClient.Id);

                return _mapper.Map<ClientOutputDto>(createdClient);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create client");
                throw;
            }
        }

        public async Task<List<ClientOutputDto>> GetAllClientsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all clients.");

                var clients = await _clientService.GetAllAsync();

                if (clients == null || !clients.Any())
                    return new List<ClientOutputDto>();

                _logger.LogInformation("Retrieved products successfully.");

                return _mapper.Map<List<ClientOutputDto>>(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve products");
                throw;
            }
        }

        public async Task<ClientOutputDto?> GetClientByCpfAsync(string sCpf)
        {
            try
            {
                var cpf = _mapper.Map<Cpf>(sCpf);

                var client = await _clientService.GetAsync(cpf);

                return _mapper.Map<ClientOutputDto>(client);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the client with CPF {cpf}", sCpf);
                throw;
            }
        }

        public async Task<ClientOutputDto?> GetClientByIdAsync(Guid id)
        {
            try
            {
                var client = await _clientService.GetAsync(id);

                return _mapper.Map<ClientOutputDto>(client);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the client with ID {id}", id);
                throw;
            }
        }

        public async Task<ClientOutputDto> UpdateClientAsync(Guid id, ClientInputDto dto)
        {
            try
            {
                var existed = await _clientService.GetAsync(id);

                _logger.LogInformation("Updating client with ID {id}.", id);

                _mapper.Map(dto, existed);

                await _clientService.UpdateAsync(existed);

                _logger.LogInformation("Client with ID {id} updated successfully.", id);

                return _mapper.Map<ClientOutputDto>(existed);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update client with ID {id}.", id);
                throw;
            }
        }

        public async Task DeleteClientAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete client with ID: {id}", id);

                await _clientService.DeleteAsync(id);

                _logger.LogInformation("Client with ID {ProductId} deleted successfully.", id);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product with ID: {ProductId}", id);
                throw;
            }
        }
    }
}