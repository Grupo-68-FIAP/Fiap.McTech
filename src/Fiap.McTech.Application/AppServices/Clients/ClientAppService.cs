using AutoMapper;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Delete;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Clients;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Clients
{
    public class ClientAppService : IClientAppService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientAppService> _logger;
        private readonly IMapper _mapper;

        public ClientAppService(IClientRepository clientRepository, ILogger<ClientAppService> logger, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ClientOutputDto> CreateClientAsync(ClientInputDto clientDto)
        {
            try
            {
                _logger.LogInformation("Creating a new client.");
                var client = new Client(clientDto.Name, new Cpf(clientDto.Cpf), new Email(clientDto.Email));
                var createdClient = await _clientRepository.AddAsync(client);
                _logger.LogInformation("Client created successfully with ID {ClientId}.", createdClient.Id);
                return _mapper.Map<ClientOutputDto>(createdClient);
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
                var clients = await _clientRepository.GetAll();
                if (clients == null || clients.Any()) return new List<ClientOutputDto>();
                _logger.LogInformation("Retrieved products successfully.");
                return _mapper.Map<List<ClientOutputDto>>(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve products");
                throw;
            }
        }

        public async Task<ClientOutputDto?> GetClientByCpfAsync(string cpf)
        {
            try
            {
                var client = await _clientRepository.GetClientByCpfAsync(new Cpf(cpf));
                if (client == null)
                {
                    _logger.LogWarning("Client with CPF {cpf} not found", cpf);
                    throw new EntityNotFoundException(string.Format("Client with CPF {0} not found.", cpf));
                }
                return _mapper.Map<ClientOutputDto>(client);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the client with CPF {cpf}", cpf);
                throw;
            }
        }

        public async Task<ClientOutputDto?> GetClientByIdAsync(Guid id)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(id);
                if (client == null)
                {
                    _logger.LogWarning("Client with ID {id} not found", id);
                    throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
                }
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
                var existed = await _clientRepository.GetByIdAsync(id);
                if (existed == null)
                {
                    _logger.LogWarning("Client with ID {id} not found. Update aborted.", id);
                    throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
                }
                _logger.LogInformation("Updating client with ID {id}.", id);
                _mapper.Map(dto, existed);
                await _clientRepository.UpdateAsync(existed);

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

                var existing = await _clientRepository.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Client with ID {id} not found. Deletion aborted.", id);
                    throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
                }

                await _clientRepository.RemoveAsync(existing);
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