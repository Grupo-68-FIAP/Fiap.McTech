using Fiap.McTech.Services.Services.MercadoPago.Models;
using Fiap.McTech.Services.Services.MercadoPago;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Moq;
using System.Text.Json;
using Fiap.McTech.UnitTests.Fakers;
using Fiap.McTech.UnitTests.Services.Utils;

namespace Fiap.McTech.UnitTests.Services
{
    public class MercadoPagoServiceTests
    {
        private readonly Mock<ILogger<MercadoPagoService>> _mockLogger;
        private readonly Mock<IOptions<MercadoPagoConfig>> _mockOptions;
        private readonly MercadoPagoConfig _mercadoPagoConfig;

        public MercadoPagoServiceTests()
        {
            _mockLogger = new Mock<ILogger<MercadoPagoService>>();
            _mercadoPagoConfig = new MercadoPagoConfig
            {
                AccessToken = Guid.NewGuid().ToString(),
                BaseUrl = "http:localhost",
                IdempotencyKey = Guid.NewGuid().ToString()
            };
            _mockOptions = new Mock<IOptions<MercadoPagoConfig>>();
            _mockOptions.Setup(opt => opt.Value).Returns(_mercadoPagoConfig);
        }

        [Fact]
        public async Task GeneratePaymentLinkAsync_ReturnsTicketUrl_WhenSuccess()
        {
            // Arrange
            var paymentRequest = PaymentServiceFaker.GeneratePaymentRequest();
            var paymentResponse = PaymentServiceFaker.GeneratePaymentResponse();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(paymentResponse))
            };

            var mockHttpMessageHandler = new MockHttpMessageHandler((request, cancellationToken) =>
            {
                return Task.FromResult(responseMessage);
            });

            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://api.mercadopago.com/")
            };

            var service = new MercadoPagoService(_mockLogger.Object, httpClient);

            // Act
            var result = await service.GeneratePaymentLinkAsync(paymentRequest);

            // Assert
            Assert.Equal(paymentResponse?.PointOfInteraction?.TransactionData?.TicketUrl, result);
        }

        [Fact]
        public async Task GeneratePaymentLinkAsync_ThrowsException_WhenResponseIsNotSuccessful()
        {
            // Arrange
            var paymentRequest = PaymentServiceFaker.GeneratePaymentRequest();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var mockHttpMessageHandler = new MockHttpMessageHandler((request, cancellationToken) =>
            {
                return Task.FromResult(responseMessage);
            });

            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://api.mercadopago.com/")
            };

            var service = new MercadoPagoService(_mockLogger.Object, httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GeneratePaymentLinkAsync(paymentRequest));
        }

        [Fact]
        public async Task GeneratePaymentLinkAsync_LogsError_WhenExceptionIsThrown()
        {
            // Arrange
            var paymentRequest = PaymentServiceFaker.GeneratePaymentRequest();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON")
            };

            var mockHttpMessageHandler = new MockHttpMessageHandler((request, cancellationToken) =>
            {
                return Task.FromResult(responseMessage);
            });

            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://api.mercadopago.com/")
            };

            var service = new MercadoPagoService(_mockLogger.Object, httpClient);

            // Act
            await Assert.ThrowsAsync<JsonException>(() => service.GeneratePaymentLinkAsync(paymentRequest));

            // Assert
            _mockLogger.Verify(
                m => m.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Falha ao gerar link de pagamento para o valor")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ReturnsTrue_WhenProcessingSucceeds()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var mockHttpMessageHandler = new MockHttpMessageHandler((request, cancellationToken) =>
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://api.mercadopago.com/")
            };
            var service = new MercadoPagoService(_mockLogger.Object, httpClient);

            // Act
            var result = await service.ProcessPaymentAsync(paymentId);

            // Assert
            Assert.True(result);
        }
    }
}
