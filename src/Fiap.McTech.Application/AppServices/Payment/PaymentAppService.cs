using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Payments;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments; 
using Fiap.McTech.Infra.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Payment
{
	public class PaymentAppService : IPaymentAppService
	{
		private readonly ILogger<PaymentAppService> _logger;
		private readonly IPaymentRepository _paymentRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IPayPalPaymentService _payPalPaymentService;

		public PaymentAppService(ILogger<PaymentAppService> logger, IPaymentRepository paymentRepository, IOrderRepository orderRepository, IPayPalPaymentService payPalPaymentService)
		{
			_logger = logger;
			_paymentRepository = paymentRepository;
			_orderRepository = orderRepository;
			_payPalPaymentService = payPalPaymentService;
		}

		public async Task<GenerateQRCodeResultDto> GenerateQRCodeAsync(Guid orderId)
		{
			try
			{
				_logger.LogInformation("Generating QR code for order with ID {OrderId}.", orderId);

				var order = await _orderRepository.GetByIdAsync(orderId);
				if (order == null)
				{
					_logger.LogInformation("Order with ID {OrderId} not found.", orderId);
					return new GenerateQRCodeResultDto(success: false, message: "Order Not Found.", null, qrCode: string.Empty);
				}

				var paymentLink = await _payPalPaymentService.GeneratePaymentLinkAsync(order.TotalAmount);
				if (string.IsNullOrEmpty(paymentLink))
				{
					_logger.LogInformation("Error to create QrCode for ID {OrderId}.", orderId);
					return new GenerateQRCodeResultDto(success: false, message: "Error to create QrCode", null, qrCode: string.Empty);
				}

				var payment = await _paymentRepository.AddAsync(new Domain.Entities.Payments.Payment(order.ClientId, order.Id, order.TotalAmount, "Nome Cliente", "Email cliente", PaymentMethod.QrCode, PaymentStatus.Pending));

				return new GenerateQRCodeResultDto(success: true, message: "QR code generated successfully.", payment.Id, qrCode: paymentLink);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to generate QR code for order with ID {OrderId}.", orderId);
				throw;
			}
		}

		public async Task<PaymentOutputDto> PayAsync(Guid paymentId, string qrCode)
		{
			try
			{
				_logger.LogInformation("Processing payment for order with ID {PaymentId}.", paymentId);

				var payment = await _paymentRepository.GetByIdAsync(paymentId);
				if (payment == null)
				{
					_logger.LogInformation("Payment with ID {OrderId} not found.", paymentId);
					return new PaymentOutputDto(success: false, message: "Payment Not Found.");
				}

				var paymentResult = await _payPalPaymentService.ProcessPaymentFromQRCodeAsync(qrCode);
				if(!paymentResult)
				{
					_logger.LogInformation("Payment failed for order with ID {OrderId}.", paymentId);

					return new PaymentOutputDto(success: false, message: "Payment failed.");
				}

				payment.UpdateStatus(PaymentStatus.Completed);
				await _paymentRepository.UpdateAsync(payment);

				_logger.LogInformation("Payment processed successfully for order with ID {OrderId}.", paymentId);

				return new PaymentOutputDto(success: true, message: "Payment processed successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process payment for order with ID {OrderId}.", paymentId);
				throw;
			}
		}
	}
}