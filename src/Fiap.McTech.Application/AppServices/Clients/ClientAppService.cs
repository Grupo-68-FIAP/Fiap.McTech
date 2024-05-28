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
    /// <summary>
    /// Application service for managing clients.
    /// </summary>
    public class ClientAppService : IClientAppService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientAppService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAppService"/> class.
        /// </summary>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientAppService(
            IClientRepository clientRepository,
            ILogger<ClientAppService> logger,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<ClientOutputDto> CreateClientAsync(ClientInputDto dto)
        {
            _logger.LogInformation("Creating a new client.");

            var newClient = _mapper.Map<Client>(dto);

            var existsClient = await _clientRepository.GetClientAsync(newClient.Cpf);
            if (existsClient != null)
                throw new EntityValidationException(string.Format("Client with CPF {0} was exists.", newClient.Cpf));

            existsClient = await _clientRepository.GetClientAsync(newClient.Email);
            if (existsClient != null)
                throw new EntityValidationException(string.Format("Client with Email {0} was exists.", newClient.Email));

            if (!newClient.IsValid())
                throw new EntityValidationException("Client invalid data.");

            var createdClient = await _clientRepository.AddAsync(newClient);

            _logger.LogInformation("Client created successfully with ID {ClientId}.", createdClient.Id);

            return _mapper.Map<ClientOutputDto>(createdClient);
        }

        /// <inheritdoc/>
        public async Task<List<ClientOutputDto>> GetAllClientsAsync()
        {
            _logger.LogInformation("Retrieving all clients.");

            var clients = await _clientRepository.GetAll();

            if (clients == null || !clients.Any())
                return new List<ClientOutputDto>();

            _logger.LogInformation("Retrieved products successfully.");

            return _mapper.Map<List<ClientOutputDto>>(clients);
        }

        /// <inheritdoc/>
        public async Task<ClientOutputDto?> GetClientByCpfAsync(string cpf)
        {
            var client = await GetAsync(_mapper.Map<Cpf>(cpf));

            return _mapper.Map<ClientOutputDto>(client);
        }

        /// <inheritdoc/>
        public async Task<ClientOutputDto?> GetClientByEmailAsync(string email)
        {
            var client = await GetAsync(_mapper.Map<Email>(email));

            return _mapper.Map<ClientOutputDto>(client);
        }

        /// <inheritdoc/>
        public async Task<ClientOutputDto?> GetClientByIdAsync(Guid id)
        {
            var client = await GetAsync(id);

            return _mapper.Map<ClientOutputDto>(client);
        }

        /// <inheritdoc/>
        public async Task<ClientOutputDto> UpdateClientAsync(Guid id, ClientInputDto dto)
        {
            var existed = await GetAsync(id);

            _logger.LogInformation("Updating client with ID {id}.", id);

            _mapper.Map(dto, existed);

            if (!existed.IsValid())
                throw new EntityValidationException("Client invalid data.");

            await _clientRepository.UpdateAsync(existed);

            _logger.LogInformation("Client with ID {id} updated successfully.", id);

            return _mapper.Map<ClientOutputDto>(existed);
        }

        /// <inheritdoc/>
        public async Task DeleteClientAsync(Guid id)
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

        private async Task<Client> GetAsync(Guid id)
        {
            return await _clientRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
        }

        private async Task<Client> GetAsync(Cpf cpf)
        {
            if (!cpf.IsValid())
                throw new EntityValidationException(string.Format("Invalid CPF {0}.", cpf.Document));

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
