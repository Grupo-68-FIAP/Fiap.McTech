using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Payments;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.Interfaces.Services;
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
					return new GenerateQRCodeResultDto(success: false, message: "Order Not Found.", qRCode: string.Empty);
				}

				var paymentLink = await _payPalPaymentService.GeneratePaymentLinkAsync(order.TotalAmount);

				//TODO - ADICIONAR PAGAMENTO NO BANCO

				return new GenerateQRCodeResultDto(success: true, message: "QR code generated successfully.", qRCode: paymentLink);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to generate QR code for order with ID {OrderId}.", orderId);
				throw;
			}
		}

		public async Task<PaymentOutputDto> PayAsync(Guid orderId, string qrCode)
		{
			try
			{
				_logger.LogInformation("Processing payment for order with ID {OrderId}.", orderId);

				var order = await _orderRepository.GetByIdAsync(orderId);
				if (order == null)
				{
					_logger.LogInformation("Order with ID {OrderId} not found.", orderId);
					return new PaymentOutputDto(success: false, message: "Order Not Found.");
				}

				var paymentResult = await _payPalPaymentService.ProcessPaymentFromQRCodeAsync(qrCode);
				if(!paymentResult)
				{
					_logger.LogInformation("Payment failed for order with ID {OrderId}.", orderId);

					return new PaymentOutputDto(success: false, message: "Payment failed.");
				}

				_logger.LogInformation("Payment processed successfully for order with ID {OrderId}.", orderId);

				//TODO - ADICIONAR PAGAMENTO NO BANCO

				return new PaymentOutputDto(success: true, message: "Payment processed successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process payment for order with ID {OrderId}.", orderId);
				throw;
			}
		}
	}
}