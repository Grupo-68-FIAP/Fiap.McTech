using Fiap.McTech.Application.AppServices.Payment;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Services.Interfaces;
using Fiap.McTech.Services.Services.MercadoPago.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.McTech.UnitTests.AppServices
{
    public class PaymentAppServiceTests
    {
        private readonly Mock<ILogger<PaymentAppService>> _mockLogger;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMercadoPagoService> _mockMercadoPagoService;
        private readonly PaymentAppService _paymentAppService;

        public PaymentAppServiceTests()
        {
            _mockLogger = new Mock<ILogger<PaymentAppService>>();
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMercadoPagoService = new Mock<IMercadoPagoService>();

            _paymentAppService = new PaymentAppService(
                _mockLogger.Object,
                _mockPaymentRepository.Object,
                _mockOrderRepository.Object,
                _mockMercadoPagoService.Object
            );
        }

        [Fact]
        public async Task GenerateQRCodeAsync_ReturnsSuccessResult_WhenQRCodeIsGenerated()
        {
            // Arrange
            var client = new Client("teste", new Cpf("12345678909"), new Email("john.doe@example.com"));
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, 100.0m, client) {};


            _mockOrderRepository
                .Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            _mockMercadoPagoService
                .Setup(service => service.GeneratePaymentLinkAsync(It.IsAny<PaymentRequest>()))
                .ReturnsAsync("https://example.com/qrcode");

            _mockPaymentRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync(new Payment(order.ClientId, order.Id, order.TotalAmount, "ClientName", "client@example.com", PaymentMethod.QrCode, PaymentStatus.Pending));

            // Act
            var result = await _paymentAppService.GenerateQRCodeAsync(orderId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("QR code generated successfully.", result.Message);
            Assert.NotEqual(Guid.Empty, result.PaymentId);
            Assert.Equal("https://example.com/qrcode", result.QRCode);
        }

        [Fact]
        public async Task GenerateQRCodeAsync_ThrowsException_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
       
            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _paymentAppService.GenerateQRCodeAsync(orderId));
        }

        [Fact]
        public async Task UpdatePayment_ReturnsSuccessResult_WhenPaymentIsProcessedSuccessfully()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), 100.0m, "ClientName", "client@example.com", PaymentMethod.QrCode, PaymentStatus.Pending);
            _mockPaymentRepository.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync(payment);
            _mockMercadoPagoService.Setup(service => service.ProcessPaymentAsync(paymentId)).ReturnsAsync(true);

            // Act
            var result = await _paymentAppService.UpdatePayment(paymentId, "completed");

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Payment processed successfully.", result.Message);
            _mockPaymentRepository.Verify(repo => repo.UpdateAsync(payment), Times.Once);
        }

        [Fact]
        public async Task UpdatePayment_ReturnsFailureResult_WhenPaymentNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
 
            // Act
            var result = await _paymentAppService.UpdatePayment(paymentId, "completed");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Payment Not Found.", result.Message);
            _mockPaymentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePayment_ReturnsFailureResult_WhenPaymentProcessingFails()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), 100.0m, "ClientName", "client@example.com", PaymentMethod.QrCode, PaymentStatus.Pending);
            _mockPaymentRepository.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync(payment);
            _mockMercadoPagoService.Setup(service => service.ProcessPaymentAsync(paymentId)).ReturnsAsync(false);

            // Act
            var result = await _paymentAppService.UpdatePayment(paymentId, "completed");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Payment failed.", result.Message);
            _mockPaymentRepository.Verify(repo => repo.UpdateAsync(payment), Times.Never);
        }
    }
}
