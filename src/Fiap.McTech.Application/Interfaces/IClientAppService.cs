using Fiap.McTech.Application.Dtos.Clients;

namespace Fiap.McTech.Application.Interfaces
{
    /// <summary>
    /// Interface for client application service.
    /// </summary>
    public interface IClientAppService
    {
        /// <summary>
        /// Retrieves all clients asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a list of client DTOs.</returns>
        Task<List<ClientOutputDto>> GetAllClientsAsync();

        /// <summary>
        /// Retrieves a client by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the client to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns a client DTO, or null if not found.</returns>
        Task<ClientOutputDto?> GetClientByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a client by CPF asynchronously.
        /// </summary>
        /// <param name="cpf">The CPF of the client to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns a client DTO, or null if not found.</returns>
        Task<ClientOutputDto?> GetClientByCpfAsync(string cpf);

        /// <summary>
        /// Creates a new client asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing client data to create.</param>
        /// <returns>A task representing the asynchronous operation that returns the created client DTO.</returns>
        Task<ClientOutputDto> CreateClientAsync(ClientInputDto dto);

        /// <summary>
        /// Updates an existing client asynchronously.
        /// </summary>
        /// <param name="id">The ID of the client to update.</param>
        /// <param name="dto">The DTO containing updated client data.</param>
        /// <returns>A task representing the asynchronous operation that returns the updated client DTO.</returns>
        Task<ClientOutputDto> UpdateClientAsync(Guid id, ClientInputDto dto);

        /// <summary>
        /// Deletes a client asynchronously.
        /// </summary>
        /// <param name="id">The ID of the client to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteClientAsync(Guid id);
    }
}
