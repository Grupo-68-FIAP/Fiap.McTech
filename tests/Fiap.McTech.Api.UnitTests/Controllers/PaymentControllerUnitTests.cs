using AutoMapper;
using Fiap.McTech.Api.Controllers.Payments;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Payment;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Infra.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.McTech.Api.UnitTests.Controllers
{
    public class PaymentControllerUnitTests
    {
        readonly Mock<IPaymentRepository> _mockedPaymentRepository;
        readonly Mock<IOrderRepository> _mockedOrderRepository;
        readonly Mock<IMercadoPagoService> _mockedmercadoPagoService;
        readonly Mock<ILogger<PaymentAppService>> _mockedLogger;

        public PaymentControllerUnitTests()
        {
            _mockedPaymentRepository = new Mock<IPaymentRepository>();
            _mockedOrderRepository = new Mock<IOrderRepository>();
            _mockedmercadoPagoService = new Mock<IMercadoPagoService>();
            _mockedLogger = new Mock<ILogger<PaymentAppService>>();
        }

        [Fact]
        public async Task GenerateQRCodeAsync_WhenOrderExists_ShouldReturnQRCodeUrl()
        {
            // Arrange
            var order = MakeNewOrder();
            order.SendToNextStatus();
            var paymentLink = "http://payment-link.com";
            var payment = new Payment(Guid.NewGuid(), order.Id, order.TotalAmount, "", "", PaymentMethod.QrCode, PaymentStatus.Pending);

            _mockedOrderRepository.Setup(x => x.GetByIdAsync(order.Id)).ReturnsAsync(order);
            _mockedmercadoPagoService.Setup(x => x.GeneratePaymentLinkAsync(order.TotalAmount)).ReturnsAsync(paymentLink);
            _mockedPaymentRepository.Setup(x => x.AddAsync(It.IsAny<Payment>())).ReturnsAsync(payment);

            var paymentController = GetPaymentController();

            // Act
            var result = await paymentController.GenerateQRCode(order.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var qrCode = Assert.IsType<GenerateQRCodeResultDto>(okResult.Value);
            Assert.Equal(paymentLink, qrCode.QRCode);
            Assert.NotNull(qrCode.PaymentId);
            _mockedPaymentRepository.Verify(x => x.AddAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task GenerateQRCodeAsync_WhenOrderDoesNotExist_ShouldThoughtException()
        {
            // Arrange
            var paymentController = GetPaymentController();
            var guid = Guid.NewGuid();

            // Act
            async Task<IActionResult> act() => await paymentController.GenerateQRCode(guid);

            // Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>((Func<Task<IActionResult>>) act);
            Assert.Contains(guid.ToString(), exception.Message);
            _mockedPaymentRepository.Verify(x => x.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task GenerateQRCodeAsync_WhenPaymentLinkIsNullOrEmpty_ShouldThoughtException()
        {
            // Arrange
            var order = MakeNewOrder();
            order.SendToNextStatus();

            _mockedOrderRepository.Setup(x => x.GetByIdAsync(order.Id)).ReturnsAsync(order);
            _mockedmercadoPagoService.Setup(x => x.GeneratePaymentLinkAsync(order.TotalAmount)).ReturnsAsync("");

            var paymentController = GetPaymentController();

            // Act
            async Task<IActionResult> act() => await paymentController.GenerateQRCode(order.Id);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>((Func<Task<IActionResult>>) act);
            Assert.Contains(order.Id.ToString(), exception.Message);
            _mockedPaymentRepository.Verify(x => x.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task PayAsync_WhenPaymentExists_ShouldReturnSuccess()
        {
            // Arrange
            var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), 10, "", "", PaymentMethod.QrCode, PaymentStatus.Pending);
            var qrCode = "qr-code";

            _mockedPaymentRepository.Setup(x => x.GetByIdAsync(payment.Id)).ReturnsAsync(payment);
            _mockedmercadoPagoService.Setup(x => x.ProcessPaymentFromQRCodeAsync(qrCode)).ReturnsAsync(true);

            var paymentController = GetPaymentController();

            // Act
            var result = await paymentController.Pay(payment.Id, qrCode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var paymentResult = Assert.IsType<PaymentOutputDto>(okResult.Value);
            Assert.True(paymentResult.Success);
            _mockedPaymentRepository.Verify(x => x.UpdateAsync(payment), Times.Once);
        }

        private static Order MakeNewOrder()
        {
            var order = new Order(null, 10);
            var item = new Order.Item(Guid.NewGuid(), order.Id, "Test", 10, 1);
            order.Items.Add(item);
            return order;
        }

        private PaymentController GetPaymentController()
        {
            var paymentAppService = new PaymentAppService(_mockedLogger.Object, _mockedPaymentRepository.Object, _mockedOrderRepository.Object, _mockedmercadoPagoService.Object);
            return new PaymentController(paymentAppService);
        }
    }
}
