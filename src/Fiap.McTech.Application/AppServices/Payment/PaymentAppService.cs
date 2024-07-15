using Fiap.McTech.Application.AppServices.Payment.Mappers;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Services.Interfaces; 
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Payment
{
    /// <summary>
    /// Represents an application service for managing payments.
    /// </summary>
    public class PaymentAppService : IPaymentAppService
    {
        private readonly ILogger<PaymentAppService> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMercadoPagoService _mercadoPagoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentAppService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging.</param>
        /// <param name="paymentRepository">The repository for managing payments.</param>
        /// <param name="orderRepository">The repository for managing orders.</param>
        /// <param name="mercadoPagoService">The Mercado Pago payment service.</param>
        public PaymentAppService(
            ILogger<PaymentAppService> logger,
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IMercadoPagoService mercadoPagoService)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _mercadoPagoService = mercadoPagoService;
        }

        /// <inheritdoc/>
        public async Task<GenerateQRCodeResultDto> GenerateQRCodeAsync(Guid orderId)
        {
            _logger.LogInformation("Generating QR code for order with ID {OrderId}.", orderId);

            var order = await _orderRepository.GetByIdAsync(orderId)
                ?? throw new EntityNotFoundException($"Order [{orderId}] not found.");

            var paymentLink = await _mercadoPagoService.GeneratePaymentLinkAsync(order.MapPaymentToServiceModel());
            if (string.IsNullOrEmpty(paymentLink))
            {
                _logger.LogInformation("Error to create QrCode for ID {OrderId}.", orderId);
                throw new InvalidOperationException($"Error to create QrCode for ID {orderId}.");
            }

            var payment = await _paymentRepository.AddAsync(new Domain.Entities.Payments.Payment(order.ClientId, order.Id, order.TotalAmount, order.Client?.Name, order.Client?.Email.ToString(), PaymentMethod.QrCode, PaymentStatus.Pending));

            return new GenerateQRCodeResultDto(success: true, message: "QR code generated successfully.", payment.Id, qrCode: paymentLink);
        }

        /// <inheritdoc/>
        public async Task<PaymentOutputDto> UpdatePayment(Guid paymentId, string status)
        {
            _logger.LogInformation("Processing payment for order with ID {PaymentId}.", paymentId);

            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogInformation("Payment with ID {OrderId} not found.", paymentId);
                return new PaymentOutputDto(success: false, message: "Payment Not Found.");
            }

            var paymentResult = await _mercadoPagoService.ProcessPaymentAsync(paymentId);
            if (!paymentResult)
            {
                _logger.LogInformation("Payment failed for order with ID {OrderId}.", paymentId);

                return new PaymentOutputDto(success: false, message: "Payment failed.");
            }

            payment.UpdateStatus(PaymentStatus.Completed);
            await _paymentRepository.UpdateAsync(payment);

            _logger.LogInformation("Payment processed successfully for order with ID {OrderId}.", paymentId);

            return new PaymentOutputDto(success: true, message: "Payment processed successfully.");
        }
    }
}
