﻿using Fiap.McTech.Infra.Services.Interfaces;
using Fiap.McTech.Services.Services.MercadoPago.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Fiap.McTech.Services.Services.MercadoPago
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly ILogger<MercadoPagoService> _logger;
        private readonly HttpClient _httpClient;
        private readonly MercadoPagoConfig _mercadoPagoConfig;

        public MercadoPagoService(
            ILogger<MercadoPagoService> logger, 
            HttpClient httpClient,
            IOptions<MercadoPagoConfig> mercadoPagoConfig
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mercadoPagoConfig = mercadoPagoConfig.Value;
        }

        public async Task<string> GeneratePaymentLinkAsync(PaymentRequest paymentRequest)
        {
            try
            {
                string json = JsonConvert.SerializeObject(paymentRequest);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

                HttpResponseMessage response = await _httpClient.PostAsync("payments", content);

                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responseString);

                return paymentResponse?.PointOfInteraction?.TransactionData?.TicketUrl ?? throw new InvalidOperationException("Falha ao recuperar o QR code.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao gerar link de pagamento para o valor {Amount}.", paymentRequest.TransactionAmount);
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
