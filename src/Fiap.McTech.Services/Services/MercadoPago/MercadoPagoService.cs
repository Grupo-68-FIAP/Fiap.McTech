﻿using Fiap.McTech.Infra.Services.Interfaces;
using Fiap.McTech.Services.Services.MercadoPago.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Fiap.McTech.Services.Services.MercadoPago
{
    [ExcludeFromCodeCoverage]
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
            catch (InvalidOperationException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while generating payment link for the amount {Amount}.", request.TransactionAmount);
                throw new InvalidOperationException("There was a problem communicating with the payment service.", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate payment link for the amount {Amount}.", request.TransactionAmount);
                return string.Empty;
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
