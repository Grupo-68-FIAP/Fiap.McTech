using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Services.Services.MercadoPago.Models;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

namespace Fiap.McTech.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class PaymentsStepDefinitions
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private PaymentRequest _paymentRequest;

        public PaymentsStepDefinitions()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            _response = new HttpResponseMessage();
            _paymentRequest = new PaymentRequest();
        }

        [Given(@"que eu tenho os detalhes do pagamento")]
        public void DadoQueEuTenhoOsDetalhesDoPagamento()
        {
            _paymentRequest = new PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                TotalAmount = 100.00m,
                Method = "CreditCard"
            };
        }

        [Given(@"que eu tenho os detalhes do pagamento com dados inválidos")]
        public void DadoQueEuTenhoOsDetalhesDoPagamentoComDadosInvalidos()
        {
            _paymentRequest = new PaymentRequest
            {
                TransactionId = Guid.Empty,
                TotalAmount = -100.00m,
                Method = ""
            };
        }

        [When(@"eu crio um novo pagamento")]
        public async Task QuandoEuCrioUmNovoPagamento()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_paymentRequest), Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("/api/payments", content);
        }

        [Then(@"o status da resposta deve ser (.*) (.*)")]
        public void EntaoOStatusDaRespostaDeveSer(int statusCode, string statusDescription)
        {
            Assert.Equal((HttpStatusCode) statusCode, _response.StatusCode);
        }

        [Then(@"o pagamento deve existir no sistema")]
        public async Task EntaoOPagamentoDeveExistirNoSistema()
        {
            var responseContent = await _response.Content.ReadAsStringAsync();
            var createdPayment = JsonConvert.DeserializeObject<PaymentOutputDto>(responseContent);
            Assert.NotNull(createdPayment);
            Assert.Equal(_paymentRequest.TransactionId, createdPayment.Id);
        }

        [Then(@"os dados do pagamento devem corresponder ao formato esperado")]
        public async Task EntaoOsDadosDoPagamentoDevemCorresponderAoFormatoEsperado()
        {
            var responseContent = await _response.Content.ReadAsStringAsync();
            var createdPayment = JsonConvert.DeserializeObject<PaymentOutputDto>(responseContent);
            Assert.Equal(_paymentRequest.TotalAmount, createdPayment.Amount);
            Assert.Equal(_paymentRequest.Method, createdPayment.PaymentMethod);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public async Task EntaoAMensagemDeErroDeveSer(string expectedErrorMessage)
        {
            var responseContent = await _response.Content.ReadAsStringAsync();
            Assert.Contains(expectedErrorMessage, responseContent);
        }

        [Given(@"um pagamento existe no sistema")]
        public async Task DadoUmPagamentoExisteNoSistema()
        {
            DadoQueEuTenhoOsDetalhesDoPagamento();
            await QuandoEuCrioUmNovoPagamento();
        }

        [Given(@"que eu tenho os novos detalhes do pagamento")]
        public void DadoQueEuTenhoOsNovosDetalhesDoPagamento()
        {
            _paymentRequest.TotalAmount = 150.00m;
            _paymentRequest.Method = "DebitCard";
        }

        [Given(@"que eu tenho os novos detalhes do pagamento com dados inválidos")]
        public void DadoQueEuTenhoOsNovosDetalhesDoPagamentoComDadosInvalidos()
        {
            _paymentRequest.TotalAmount = -150.00m;
            _paymentRequest.Method = "";
        }

        [When(@"eu atualizo o pagamento")]
        public async Task QuandoEuAtualizoOPagamento()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_paymentRequest), Encoding.UTF8, "application/json");
            _response = await _client.PutAsync($"/api/payments/{_paymentRequest.TransactionId}", content);
        }

        [When(@"eu removo o pagamento")]
        public async Task QuandoEuRemovoOPagamento()
        {
            _response = await _client.DeleteAsync($"/api/payments/{_paymentRequest.TransactionId}");
        }

        [Given(@"que eu tenho um ID de pagamento inexistente")]
        public void DadoQueEuTenhoUmIDDePagamentoInexistente()
        {
            _paymentRequest.TransactionId = Guid.NewGuid();
        }
    }

    public class PaymentRequest
    {
        public Guid TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Method { get; set; }
    }

    public class PaymentOutputDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
