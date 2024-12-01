using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Cart;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.ValuesObjects;
using Moq;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Application.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Api.Controllers.Cart;

namespace Fiap.McTech.FunctionalTests.StepDefinitions
{
    [Binding]
    public class CartStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IClientRepository> _mockedClientRepository;
        private readonly Mock<ICartClientRepository> _mockedCartClientRepository;
        private readonly ClientAppService _clientAppService;
        private readonly CartAppService _cartAppService;
        private readonly CartController _controller;

        public CartStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockedClientRepository = new Mock<IClientRepository>();
            _mockedCartClientRepository = new Mock<ICartClientRepository>();
            _clientAppService = new ClientAppService(_mockedClientRepository.Object, new Mock<ILogger<ClientAppService>>().Object, new MapperConfiguration(cfg => cfg.AddProfile(new ClientProfile())).CreateMapper());
            _cartAppService = new CartAppService(_mockedCartClientRepository.Object, new Mock<IProductAppService>().Object, _clientAppService, new Mock<ILogger<CartAppService>>().Object, new MapperConfiguration(cfg => cfg.AddProfile(new CartClientProfile())).CreateMapper());
            _controller = new CartController(_cartAppService, _clientAppService);
        }

        [Given(@"que eu tenho um cliente autenticado")]
        public async Task DadoQueEuTenhoUmClienteAutenticado()
        {
            var client = new Client("Cliente Teste", new Cpf("12345678900"), new Email("cliente@teste.com"));
            _scenarioContext["Client"] = client;
        }

        [Given(@"que eu tenho os detalhes do carrinho com cliente inexistente")]
        public void DadoQueEuTenhoOsDetalhesDoCarrinhoComClienteInexistente()
        {
            var cartDto = new CartClientInputDto
            {
                ClientId = Guid.NewGuid(),
                Items = new List<CartClientInputDto.Item>
                {
                    new CartClientInputDto.Item { ProductId = Guid.NewGuid(), Quantity = 5 }
                }
            };
            _scenarioContext["CartDto"] = cartDto;
        }

        [Given(@"que eu tenho os detalhes do carrinho")]
        public void DadoQueEuTenhoOsDetalhesDoCarrinho()
        {
            var client = (Client) _scenarioContext["Client"];
            var cartDto = new CartClientInputDto
            {
                ClientId = client.Id,
                Items = new List<CartClientInputDto.Item>
                {
                    new CartClientInputDto.Item { ProductId = Guid.NewGuid(), Quantity = 5 }
                }
            };
            _scenarioContext["CartDto"] = cartDto;
        }

        [When(@"eu crio um novo carrinho")]
        public async Task QuandoEuCrioUmNovoCarrinho()
        {
            var cartDto = (CartClientInputDto) _scenarioContext["CartDto"];
            try
            {
                var result = await _controller.CreateCart(cartDto, "Bearer token");
                _scenarioContext["Response"] = result;
                _scenarioContext["ResponseStatus"] = ((ObjectResult) result.Result).StatusCode;
            }
            catch (EntityNotFoundException ex)
            {
                _scenarioContext["ResponseStatus"] = 404;
                _scenarioContext["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                _scenarioContext["ResponseStatus"] = 500;
            }
        }

        [Then(@"o status da resposta deve ser (.*) Created")]
        public void EntaoOStatusDaRespostaDeveSerCreated(int statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext["ResponseStatus"]);
        }

        [Then(@"o carrinho deve existir no sistema")]
        public async Task EntaoOCarrinhoDeveExistirNoSistema()
        {
            var cartDto = (CartClientInputDto) _scenarioContext["CartDto"];
            var result = await _controller.GetCartByClientId(cartDto.ClientId.Value) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
        }

        [Then(@"os dados do carrinho devem corresponder ao formato esperado")]
        public void EntaoOsDadosDoCarrinhoDevemCorresponderAoFormatoEsperado()
        {
            var response = (ObjectResult) _scenarioContext["Response"];
            var cart = response.Value as CartClientOutputDto;
            var cartDto = (CartClientInputDto) _scenarioContext["CartDto"];
            Assert.NotNull(cart);
            Assert.Equal(cartDto.ClientId, cart.ClientId);
            Assert.Equal(cartDto.Items.Count, cart.Items.Count);
        }

        [When(@"eu adiciono o produto ao carrinho")]
        public async Task QuandoEuAdicionoOProdutoAoCarrinho()
        {
            var cart = (CartClient) _scenarioContext["Cart"];
            var product = (dynamic) _scenarioContext["Product"];
            try
            {
                await _controller.AddCartItemToCartClientAsync(cart.Id, product.ProductId, product.Quantity);
                _scenarioContext["ResponseStatus"] = 200;
            }
            catch (EntityNotFoundException ex)
            {
                _scenarioContext["ResponseStatus"] = 404;
                _scenarioContext["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                _scenarioContext["ResponseStatus"] = 500;
            }
        }

        [When(@"eu atualizo a quantidade de um item no carrinho")]
        public async Task QuandoEuAtualizoAQuantidadeDeUmItemNoCarrinho()
        {
            var cart = (CartClient) _scenarioContext["Cart"];
            var product = (dynamic) _scenarioContext["Product"];
            try
            {
                await _controller.AddCartItemToCartClientAsync(cart.Id, product.ProductId, 5);
                _scenarioContext["ResponseStatus"] = 200;
            }
            catch (EntityNotFoundException ex)
            {
                _scenarioContext["ResponseStatus"] = 404;
                _scenarioContext["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                _scenarioContext["ResponseStatus"] = 500;
            }
        }

        [Then(@"a quantidade do item deve ser atualizada no carrinho")]
        public void EntaoAQuantidadeDoItemDeveSerAtualizadaNoCarrinho()
        {
            var cart = (CartClient) _scenarioContext["Cart"];
            _mockedCartClientRepository.Verify(repo => repo.UpdateAsync(It.IsAny<CartClient>()), Times.Once);
        }

        [Given(@"que eu tenho os detalhes do carrinho sem cliente")]
        public void DadoQueEuTenhoOsDetalhesDoCarrinhoSemCliente()
        {
            var cartDto = new CartClientInputDto
            {
                Items = new List<CartClientInputDto.Item>
                {
                    new CartClientInputDto.Item { ProductId = Guid.NewGuid(), Quantity = 5 }
                }
            };
            _scenarioContext["CartDto"] = cartDto;
        }

        [Given(@"que um carrinho existe no sistema")]
        public Task DadoQueUmCarrinhoExisteNoSistema()
        {
            var client = (Client) _scenarioContext["Client"];
            var cart = new CartClient(client.Id, 0);
            _mockedCartClientRepository.Setup(repo => repo.GetByClientIdAsync(client.Id)).ReturnsAsync(cart);
            _scenarioContext["Cart"] = cart;
            return Task.CompletedTask;
        }

        [Given(@"que eu tenho os detalhes do produto")]
        public void DadoQueEuTenhoOsDetalhesDoProduto()
        {
            var product = new { ProductId = Guid.NewGuid(), Quantity = 5 };
            _scenarioContext["Product"] = product;
        }

        [Then(@"o produto deve ser adicionado ao carrinho")]
        public void EntaoOProdutoDeveSerAdicionadoAoCarrinho()
        {
            var cart = (CartClient) _scenarioContext["Cart"];
            var product = (dynamic) _scenarioContext["Product"];
            Assert.Contains(cart.Items, item => item.ProductId == product.ProductId && item.Quantity == product.Quantity);
        }

        [Then(@"os dados do carrinho devem ser atualizados no sistema")]
        public void EntaoOsDadosDoCarrinhoDevemSerAtualizadosNoSistema()
        {
            var cart = (CartClient) _scenarioContext["Cart"];
            _mockedCartClientRepository.Verify(repo => repo.UpdateAsync(cart), Times.Once);
        }

        [Given(@"que um carrinho com itens existe para um cliente")]
        public Task DadoQueUmCarrinhoComItensExisteParaUmCliente()
        {
            var client = (Client) _scenarioContext["Client"];
            var cart = new CartClient(client.Id, 0);
            cart.Items.Add(new CartClient.Item("Nome do Produto", 5, 0, Guid.NewGuid(), Guid.NewGuid()));
            _mockedCartClientRepository.Setup(repo => repo.GetByClientIdAsync(client.Id)).ReturnsAsync(cart);
            _scenarioContext["Cart"] = cart;
            return Task.CompletedTask;
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public void EntaoAMensagemDeErroDeveSer(string mensagemDeErro)
        {
            Assert.Equal(mensagemDeErro, _scenarioContext["ErrorMessage"]);
        }
    }
}
