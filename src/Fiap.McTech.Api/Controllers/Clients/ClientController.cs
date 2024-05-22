using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Clients
{
    /// <summary>
    /// Controller for managing clients.
    /// </summary>
    [ApiController]
    [Route("api/client")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ClientController : Controller
    {
        private readonly IClientAppService _clientAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        /// <param name="clientAppService">The client application service.</param>
        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        /// <returns>A list of clients.</returns>
        /// <response code="200">Returns all clients.</response>
        /// <response code="204">If no clients are found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientAppService.GetAllClientsAsync();
            return (clients == null || !clients.Any()) ? new NoContentResult() : Ok(clients);
        }

        /// <summary>
        /// Retrieves a client by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <returns>The requested client.</returns>
        /// <response code="200">Returns the specified client.</response>
        /// <response code="404">If the client with the given ID is not found.</response>
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(ClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClient(Guid id)
        {
            try
            {
                return Ok(await _clientAppService.GetClientByIdAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a client by its CPF (Brazilian Taxpayer Registry).
        /// </summary>
        /// <param name="cpf">The CPF of the client.</param>
        /// <returns>The requested client.</returns>
        /// <response code="200">Returns the specified client.</response>
        /// <response code="400">If there are validation issues with the CPF.</response>
        /// <response code="404">If the client with the given CPF is not found.</response>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(typeof(ClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClientByCpf(string cpf)
        {
            try
            {
                return Ok(await _clientAppService.GetClientByCpfAsync(cpf));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="client">The data of the client to be created.</param>
        /// <returns>The created client.</returns>
        /// <response code="201">Returns the newly created client.</response>
        /// <response code="400">If there are validation issues with the input data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClientOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClient(ClientInputDto client)
        {
            var createdClient = await _clientAppService.CreateClientAsync(client);
            return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
        }

        /// <summary>
        /// Updates an existing client.
        /// </summary>
        /// <param name="id">The unique identifier of the client to be updated.</param>
        /// <param name="client">The updated data of the client.</param>
        /// <returns>The updated client.</returns>
        /// <response code="200">Returns the updated client.</response>
        /// <response code="400">If there are validation issues with the updated data.</response>
        /// <response code="404">If the client with the given ID is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClient(Guid id, ClientInputDto client)
        {
            try
            {
                return Ok(await _clientAppService.UpdateClientAsync(id, client));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an existing client.
        /// </summary>
        /// <param name="id">The unique identifier of the client to be deleted.</param>
        /// <response code="204">Indicates successful deletion of the client.</response>
        /// <response code="404">If the client with the given ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                await _clientAppService.DeleteClientAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }
    }
}
