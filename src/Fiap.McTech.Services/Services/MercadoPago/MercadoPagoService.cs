using Fiap.McTech.Infra.Services.Interfaces;
using Fiap.McTech.Services.Services.MercadoPago.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Fiap.McTech.Services.Services.MercadoPago
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly ILogger<MercadoPagoService> _logger;
        private readonly HttpClient _httpClient;

        public MercadoPagoService(
            ILogger<MercadoPagoService> logger, 
            HttpClient httpClient
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GeneratePaymentLinkAsync(PaymentRequest request)
        {
            try
            {
                string json = JsonSerializer.Serialize(request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("v1/payments", content);

                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseString);

                return paymentResponse?.PointOfInteraction?.TransactionData?.TicketUrl ?? throw new InvalidOperationException("Falha ao recuperar o QR code.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao gerar link de pagamento para o valor {Amount}.", request.TransactionAmount);
                throw;
            }
        }

        public async Task<bool> ProcessPaymentAsync(Guid paymentId)
        {
            try
            {
                _logger.LogInformation("Processing payment from paymentId: {paymentId}.", paymentId);

                //MOCK VALUE
                return await Task.Run(() => { return true; });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process payment from paymentId: {paymentId}.", paymentId);

                return await Task.Run(() => { return false; });
            }
        }
    }
}
