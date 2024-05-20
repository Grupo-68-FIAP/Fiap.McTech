using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Clients
{
    [ApiController]
    [Route("api/client")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ClientController : Controller
    {
        private readonly IClientAppService _clientAppService;

        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        /// <summary>
        /// Obtain all clients.
        /// </summary>
        /// <returns>List of clients</returns>
        /// <response code="200">Returns all items</response>
        /// <response code="204">If there are nothing</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientAppService.GetAllClientsAsync();
            return (clients == null || !clients.Any()) ? new NoContentResult() : Ok(clients);
        }

        /// <summary>
        /// Obtain client by id
        /// </summary>
        /// <param name="id">Guid of reference that client</param>
        /// <returns>Return client</returns>
        /// <response code="200">Returns item</response>
        /// <response code="404">If client isn't exists</response>
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
        /// Obtain client by CPF
        /// </summary>
        /// <param name="cpf">CPF of client</param>
        /// <returns>Return client</returns>
        /// <response code="200">Return item</response>
        /// <response code="400">If there validations issues</response>
        /// <response code="404">If client isn't exists</response>
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
        /// Create a new client
        /// </summary>
        /// <param name="client">Input data of client</param>
        /// <returns>Return client</returns>
        /// <response code="201">Return new client</response>
        /// <response code="400">If there validations issues</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClientOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClient(ClientInputDto client)
        {
            try
            {
                var createdClient = await _clientAppService.CreateClientAsync(client);
                return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing client
        /// </summary>
        /// <param name="client">Input data of client</param>
        /// <returns>Return client</returns>
        /// <response code="200">Return new client</response>
        /// <response code="400">If there validations issues</response>
        /// <response code="404">If client isn't exists</response>
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
        /// Remove an existing client
        /// </summary>
        /// <param name="client">Input data of client</param>
        /// <returns>Return client</returns>
        /// <response code="204">Return new client</response>
        /// <response code="404">If client isn't exists</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id)
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
