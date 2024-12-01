using Fiap.McTech.Api.Controllers.Clients;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace FunctionalTests.StepDefinitions
{
    [Binding]
    public class ClientsStepDefinitions
    {
        private readonly ClientController _controller;
        private IActionResult? _response;
        private Guid _clientId;
        private ClientInputDto? _clientInputDto;
        private ClientInputDto? _invalidClientInputDto;

        public ClientsStepDefinitions(IClientAppService clientAppService)
        {
            _controller = new ClientController(clientAppService);
        }

        [Given(@"que eu tenho um ID de cliente único")]
        public void GivenQueEuTenhoUmIDDeClienteUnico()
        {
            _clientId = Guid.NewGuid();
        }

        [Given(@"que eu tenho os detalhes do cliente")]
        public void GivenQueEuTenhoOsDetalhesDoCliente()
        {
            _clientInputDto = new ClientInputDto("Cliente Teste", "12345678901", "cliente@teste.com");
        }

        [Given(@"que eu tenho os detalhes do cliente com CPF inválido")]
        public void GivenQueEuTenhoOsDetalhesDoClienteComCPFInvalido()
        {
            _invalidClientInputDto = new ClientInputDto("Cliente Teste", "CPFInvalido", "cliente@teste.com");
        }

        [When(@"eu crio um novo cliente")]
        public async Task WhenEuCrioUmNovoCliente()
        {
            _response = await _controller.CreateClient(_clientInputDto);
        }

        [When(@"eu crio um novo cliente com CPF inválido")]
        public async Task WhenEuCrioUmNovoClienteComCPFInvalido()
        {
            _response = await _controller.CreateClient(_invalidClientInputDto);
        }

        [Then(@"o status da resposta deve ser (.*) Created")]
        public void ThenOStatusDaRespostaDeveSerCreated(int statusCode)
        {
            var createdAtResult = _response as CreatedAtActionResult;
            Assert.Equal(statusCode, createdAtResult?.StatusCode);
        }

        [Then(@"o status da resposta deve ser (.*) BadRequest")]
        public void ThenOStatusDaRespostaDeveSerBadRequest(int statusCode)
        {
            var badRequestResult = _response as BadRequestObjectResult;
            Assert.Equal(statusCode, badRequestResult?.StatusCode);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public void ThenAMensagemDeErroDeveSer(string errorMessage)
        {
            var badRequestResult = _response as BadRequestObjectResult;
            Assert.Contains(errorMessage, badRequestResult?.Value.ToString());
        }

        [Then(@"o cliente deve existir no sistema")]
        public async Task ThenOClienteDeveExistirNoSistema()
        {
            var result = await _controller.GetClient(_clientId) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int) HttpStatusCode.OK, result?.StatusCode);
        }

        [When(@"eu recupero o cliente")]
        public async Task WhenEuRecuperoOCliente()
        {
            _response = await _controller.GetClient(_clientId);
        }

        [Then(@"os dados do cliente devem corresponder ao formato esperado")]
        public void ThenOsDadosDoClienteDevemCorresponderAoFormatoEsperado()
        {
            var client = ((OkObjectResult) _response)?.Value as ClientOutputDto;
            Assert.NotNull(client);
            Assert.Equal(_clientInputDto?.Name, client?.Name);
            Assert.Equal(_clientInputDto?.Cpf, client?.Cpf);
            Assert.Equal(_clientInputDto?.Email, client?.Email);
        }

        [Given(@"que um cliente existe no sistema")]
        public async Task GivenQueUmClienteExisteNoSistema()
        {
            _clientInputDto = new ClientInputDto("Cliente Existente", "12345678901", "cliente@existente.com");
            var result = await _controller.CreateClient(_clientInputDto) as CreatedAtActionResult;
            _clientId = (Guid) (result?.Value ?? Guid.Empty);
        }

        [Given(@"que eu tenho os novos detalhes do cliente")]
        public void GivenQueEuTenhoOsNovosDetalhesDoCliente()
        {
            _clientInputDto = new ClientInputDto("Cliente Atualizado", "12345678901", "cliente@atualizado.com");
        }

        [Given(@"que eu tenho os novos detalhes do cliente com CPF inválido")]
        public void GivenQueEuTenhoOsNovosDetalhesDoClienteComCPFInvalido()
        {
            _invalidClientInputDto = new ClientInputDto("Cliente Atualizado", "CPFInvalido", "cliente@atualizado.com");
        }

        [When(@"eu atualizo o cliente")]
        public async Task WhenEuAtualizoOCliente()
        {
            _response = await _controller.UpdateClient(_clientId, _clientInputDto);
        }

        [When(@"eu atualizo o cliente com CPF inválido")]
        public async Task WhenEuAtualizoOClienteComCPFInvalido()
        {
            _response = await _controller.UpdateClient(_clientId, _invalidClientInputDto);
        }

        [Then(@"o status da resposta deve ser (.*) OK")]
        public void ThenOStatusDaRespostaDeveSerOK(int statusCode)
        {
            var okResult = _response as OkObjectResult;
            Assert.Equal(statusCode, okResult?.StatusCode);
        }

        [Then(@"os dados do cliente devem ser atualizados no sistema")]
        public async Task ThenOsDadosDoClienteDevemSerAtualizadosNoSistema()
        {
            var result = await _controller.GetClient(_clientId) as OkObjectResult;
            var updatedClient = result?.Value as ClientOutputDto;
            Assert.NotNull(updatedClient);
            Assert.Equal(_clientInputDto?.Name, updatedClient?.Name);
            Assert.Equal(_clientInputDto?.Cpf, updatedClient?.Cpf);
            Assert.Equal(_clientInputDto?.Email, updatedClient?.Email);
        }

        [When(@"eu removo o cliente")]
        public async Task WhenEuRemovoOCliente()
        {
            _response = await _controller.DeleteClient(_clientId);
        }

        [Then(@"o status da resposta deve ser (.*) No Content")]
        public void ThenOStatusDaRespostaDeveSerNoContent(int statusCode)
        {
            var noContentResult = _response as NoContentResult;
            Assert.Equal(statusCode, noContentResult?.StatusCode);
        }

        [Then(@"o cliente não deve mais existir no sistema")]
        public async Task ThenOClienteNaoDeveMaisExistirNoSistema()
        {
            var result = await _controller.GetClient(_clientId) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Given(@"que eu tenho um ID de cliente inexistente")]
        public void GivenQueEuTenhoUmIDDeClienteInexistente()
        {
            _clientId = Guid.NewGuid();
        }

        [Then(@"o status da resposta deve ser (.*) Not Found")]
        public void ThenOStatusDaRespostaDeveSerNotFound(int statusCode)
        {
            var notFoundResult = _response as NotFoundObjectResult;
            Assert.Equal(statusCode, notFoundResult?.StatusCode);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public void ThenAMensagemDeErroDeveSerClienteNaoEncontrado(string errorMessage)
        {
            var notFoundResult = _response as NotFoundObjectResult;
            Assert.Contains(errorMessage, notFoundResult?.Value.ToString());
        }
    }
}
