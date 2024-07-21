using Fiap.McTech.Api.Controllers.Payments;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fiap.McTech.Api.UnitTests.Controllers
{
    public class PaymentControllerUnitTests
    {
        private readonly Mock<IPaymentAppService> _mockPaymentAppService;
        private readonly PaymentController _controller;

        public PaymentControllerUnitTests()
        {
            _mockPaymentAppService = new Mock<IPaymentAppService>();
            _controller = new PaymentController(_mockPaymentAppService.Object);
        }

        [Fact]
        public async Task GenerateQRCode_ReturnsOkResult_WithQrCodeUrl()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var expectedQrCodeUrl = "https://example.com/qrcode";

            _mockPaymentAppService
                .Setup(service => service.GenerateQRCodeAsync(paymentId))
                .ReturnsAsync(new GenerateQRCodeResultDto(true, "Sucesso", paymentId, expectedQrCodeUrl));

            // Act
            var result = await _controller.GenerateQRCode(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GenerateQRCode_ReturnsBadRequestResult_WhenSuccessIsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var expectedResult = new GenerateQRCodeResultDto(false, "Failed to generate QR code", Guid.Empty, "");

            _mockPaymentAppService
                .Setup(service => service.GenerateQRCodeAsync(orderId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GenerateQRCode(orderId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
