using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fiap.McTech.Domain.Entities.Orders;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

namespace Fiap.McTech.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class OrdersStepDefinitions
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private string? _orderDetails;
        private string? _clientDetails;

        public OrdersStepDefinitions()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        }

        [Given(@"que eu tenho um cliente autenticado")]
        public void DadoQueEuTenhoUmClienteAutenticado()
        {
            _clientDetails = JsonConvert.SerializeObject(new { ClientId = Guid.NewGuid(), AuthToken = "valid-token" });
        }

        [Given(@"que eu tenho os detalhes do pedido")]
        public void DadoQueEuTenhoOsDetalhesDoPedido()
        {
            _orderDetails = JsonConvert.SerializeObject(new { ProductId = Guid.NewGuid(), Quantity = 2 });
        }

        [When(@"eu crio um novo pedido")]
        public async Task QuandoEuCrioUmNovoPedido()
        {
            var content = new StringContent(_orderDetails, Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("/api/orders", content);
        }

        [Then(@"o status da resposta deve ser (.*) Created")]
        public void EntaoOStatusDaRespostaDeveSerCreated(int statusCode)
        {
            Assert.Equal((HttpStatusCode) statusCode, _response.StatusCode);
        }

        [Then(@"o pedido deve existir no sistema")]
        public async Task EntaoOPedidoDeveExistirNoSistema()
        {
            var response = await _client.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();
            var orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
            Assert.Contains(orders, o => o.ClientId == Guid.Parse("12345"));
        }

        [Then(@"os dados do pedido devem corresponder ao formato esperado")]
        public async Task EntaoOsDadosDoPedidoDevemCorresponderAoFormatoEsperado()
        {
            var response = await _client.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();
            var orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
            var order = orders.First(o => o.ClientId == Guid.Parse("12345"));
            Assert.Equal(Guid.Parse("67890"), order.Id);
            Assert.Equal(2, order.Items.Count);
        }

        [Given(@"que eu tenho os detalhes do pedido sem cliente")]
        public void DadoQueEuTenhoOsDetalhesDoPedidoSemCliente()
        {
            _orderDetails = JsonConvert.SerializeObject(new { ProductId = Guid.NewGuid(), Quantity = 2 });
        }

        [Then(@"o status da resposta deve ser (.*) BadRequest")]
        public void EntaoOStatusDaRespostaDeveSerBadRequest(int statusCode)
        {
            Assert.Equal((HttpStatusCode) statusCode, _response.StatusCode);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public async Task EntaoAMensagemDeErroDeveSer(string mensagemDeErro)
        {
            var content = await _response.Content.ReadAsStringAsync();
            Assert.Contains(mensagemDeErro, content);
        }

        [Given(@"um pedido existe no sistema")]
        public async Task DadoUmPedidoExisteNoSistema()
        {
            var content = new StringContent(_orderDetails, Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("/api/orders", content);
            _response.EnsureSuccessStatusCode();
        }

        [Given(@"que eu tenho os detalhes do item")]
        public void DadoQueEuTenhoOsDetalhesDoItem()
        {
            _orderDetails = JsonConvert.SerializeObject(new { ProductId = Guid.NewGuid(), Quantity = 1 });
        }

        [When(@"eu adiciono o item ao pedido")]
        public async Task QuandoEuAdicionoOItemAoPedido()
        {
            var content = new StringContent(_orderDetails, Encoding.UTF8, "application/json");
            _response = await _client.PutAsync("/api/orders/12345/items", content);
        }

        [Then(@"o item deve ser adicionado ao pedido")]
        public async Task EntaoOItemDeveSerAdicionadoAoPedido()
        {
            var response = await _client.GetAsync("/api/orders/12345");
            response.EnsureSuccessStatusCode();
            var order = JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync());
            Assert.Contains(order.Items, i => i.ProductId == Guid.Parse("67890"));
        }

        [Then(@"os dados do pedido devem ser atualizados no sistema")]
        public async Task EntaoOsDadosDoPedidoDevemSerAtualizadosNoSistema()
        {
            var response = await _client.GetAsync("/api/orders/12345");
            response.EnsureSuccessStatusCode();
            var order = JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync());
            Assert.Equal(3, order.Items.Count);
        }

        [Given(@"que eu tenho os detalhes do pedido com produto inexistente")]
        public void DadoQueEuTenhoOsDetalhesDoPedidoComProdutoInexistente()
        {
            _orderDetails = JsonConvert.SerializeObject(new { ProductId = Guid.NewGuid(), Quantity = 2 });
        }

        [Then(@"o status da resposta deve ser (.*) NotFound")]
        public void EntaoOStatusDaRespostaDeveSerNotFound(int statusCode)
        {
            Assert.Equal((HttpStatusCode) statusCode, _response.StatusCode);
        }

        [Given(@"que eu tenho os detalhes do pedido com cliente inexistente")]
        public void DadoQueEuTenhoOsDetalhesDoPedidoComClienteInexistente()
        {
            _clientDetails = JsonConvert.SerializeObject(new { ClientId = Guid.NewGuid(), AuthToken = "valid-token" });
        }

        [Given(@"que eu tenho os novos detalhes do pedido")]
        public void DadoQueEuTenhoOsNovosDetalhesDoPedido()
        {
            _orderDetails = JsonConvert.SerializeObject(new { ProductId = Guid.NewGuid(), Quantity = 3 });
        }

        [When(@"eu atualizo o pedido")]
        public async Task QuandoEuAtualizoOPedido()
        {
            var content = new StringContent(_orderDetails, Encoding.UTF8, "application/json");
            _response = await _client.PutAsync("/api/orders/12345", content);
        }

        [When(@"eu removo o pedido")]
        public async Task QuandoEuRemovoOPedido()
        {
            _response = await _client.DeleteAsync("/api/orders/12345");
        }

        [Then(@"o pedido não deve mais existir no sistema")]
        public async Task EntaoOPedidoNaoDeveMaisExistirNoSistema()
        {
            var response = await _client.GetAsync("/api/orders/12345");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Given(@"que eu tenho um ID de pedido inexistente")]
        public void DadoQueEuTenhoUmIDDePedidoInexistente()
        {
            _orderDetails = "99999";
        }
    }
}
