using AutoMapper;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Clients;
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

        public ClientAppService(
            IClientRepository clientRepository,
            ILogger<ClientAppService> logger,
            IMapper mapper)
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

                var newClient = _mapper.Map<Client>(clientDto);

                var existsClient = await _clientRepository.GetClientAsync(newClient.Cpf);
                if (existsClient != null) throw new EntityValidationException(string.Format("Client with CPF {0} was exists.", newClient.Cpf));

                existsClient = await _clientRepository.GetClientAsync(newClient.Email);
                if (existsClient != null) throw new EntityValidationException(string.Format("Client with Email {0} was exists.", newClient.Email));

                if (!newClient.IsValid()) throw new EntityValidationException("Client invalid data.");

                var createdClient = await _clientRepository.AddAsync(newClient);

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

                var clients = await _clientRepository.GetAll();

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

                var client = await GetAsync(cpf);

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
                var client = await GetAsync(id);

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
                var existed = await GetAsync(id);

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

                var existing = await GetAsync(id);
                try
                {
                    await _clientRepository.RemoveAsync(existing);
                }
                catch (Exception ex)
                {
                    throw new DatabaseException("It can't possivel remove entity client.", ex);
                }

                _logger.LogInformation("Client with ID {ProductId} deleted successfully.", id);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product with ID: {ProductId}", id);
                throw;
            }
        }

        private async Task<Client> GetAsync(Guid id)
        {
            return await _clientRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
        }

        private async Task<Client> GetAsync(Cpf cpf)
        {
            if (!cpf.IsValid())
                throw new EntityValidationException(string.Format("Invalid CPF {0}.", cpf));

            return await _clientRepository.GetClientAsync(cpf)
                ?? throw new EntityNotFoundException(string.Format("Client with CPF {0} not found.", cpf));
        }

        private async Task<Client> GetAsync(Email email)
        {
            if (!email.IsValid())
                throw new EntityValidationException(string.Format("Invalid Email {0}.", email));

            return await _clientRepository.GetClientAsync(email)
                ?? throw new EntityNotFoundException(string.Format("Client with Email {0} not found.", email));
        }
    }
}