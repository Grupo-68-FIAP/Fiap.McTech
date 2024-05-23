using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using System.Net;

namespace Fiap.McTech.Domain.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAll();
        }

        public async Task<Client> GetAsync(Guid id)
        {
            return await _clientRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(string.Format("Client with ID {0} not found.", id));
        }

        public async Task<Client> GetAsync(Cpf cpf)
        {
            if (!cpf.IsValid())
                throw new EntityValidationException(string.Format("Invalid CPF {0}.", cpf));

            return await _clientRepository.GetClientAsync(cpf)
                ?? throw new EntityNotFoundException(string.Format("Client with CPF {0} not found.", cpf));
        }

        public async Task<Client> GetAsync(Email email)
        {
            if (!email.IsValid())
                throw new EntityValidationException(string.Format("Invalid Email {0}.", email));

            return await _clientRepository.GetClientAsync(email)
                ?? throw new EntityNotFoundException(string.Format("Client with Email {0} not found.", email));
        }

        public async Task<Client> CreateAsync(Client newClient)
        {
            var existsClient = await _clientRepository.GetClientAsync(newClient.Cpf);
            if (existsClient != null) throw new EntityValidationException(string.Format("Client with CPF {0} was exists.", newClient.Cpf));

            existsClient = await _clientRepository.GetClientAsync(newClient.Email);
            if (existsClient != null) throw new EntityValidationException(string.Format("Client with Email {0} was exists.", newClient.Email));

            if (!newClient.IsValid()) throw new EntityValidationException("Client invalid data.");

            return await _clientRepository.AddAsync(newClient);
        }

        public async Task<Client> UpdateAsync(Client updateClient)
        {
            await GetAsync(updateClient.Id);

            await _clientRepository.UpdateAsync(updateClient);

            return updateClient;
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await GetAsync(id);
            try
            {
                await _clientRepository.RemoveAsync(existing);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("It can't possivel remove entity client.", ex);
            }

            await Task.Run(() => { });
        }
    }
}
