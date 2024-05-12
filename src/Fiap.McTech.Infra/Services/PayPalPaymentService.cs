using Fiap.McTech.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Infra.Services
{
	public class PayPalPaymentService : IPayPalPaymentService
	{
		private readonly ILogger<PayPalPaymentService> _logger;

		public PayPalPaymentService(ILogger<PayPalPaymentService> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<string> GeneratePaymentLinkAsync(decimal amount)
		{
			try
			{
				_logger.LogInformation("Generating payment link for amount {Amount}.", amount);

				//MOCK VALUE
				return $"https://www.paypal.com/payment?amount={amount}";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to generate payment link for amount {Amount}.", amount);
				throw;
			}
		}

		public async Task<bool> ProcessPaymentFromQRCodeAsync(string qrCode)
		{
			try
			{
				_logger.LogInformation("Processing payment from QR code: {QRCode}.", qrCode);

				//MOCK VALUE
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process payment from QR code: {QRCode}.", qrCode);

				return false;
			}
		}
	}
}