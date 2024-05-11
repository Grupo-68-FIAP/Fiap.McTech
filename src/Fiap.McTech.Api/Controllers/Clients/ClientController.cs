using Fiap.McTech.Application.AppServices.Product;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Clients
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : Controller
    {
        private readonly IClientAppService _clientAppService;

        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        [HttpGet]
        public async Task<ActionResult<ClientOutputDto>> GetAllClients()
        {
            return Ok(await _clientAppService.GetAllClientsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientOutputDto>> GetClient(Guid id)
        {
            var client = await _clientAppService.GetClientByIdAsync(id);
            return client == null ? NotFound() : Ok(client);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<ClientOutputDto>> GetClientByCpf(string cpf)
        {
            var client = await _clientAppService.GetClientByCpfAsync(cpf);
            return client == null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<ClientOutputDto>> CreateClient(ClientInputDto id)
        {
            var createdClient = await _clientAppService.CreateClientAsync(id);
            return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, ClientInputDto dto)
        {
            var updated = await _clientAppService.UpdateClientAsync(id, dto);
            return updated == null ? NotFound() : Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _clientAppService.DeleteClientAsync(id);
            return NoContent();
        }
    }
}
