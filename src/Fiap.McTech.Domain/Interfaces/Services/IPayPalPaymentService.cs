namespace Fiap.McTech.Domain.Interfaces.Services
{
	public interface IPayPalPaymentService
	{
		Task<string> GeneratePaymentLinkAsync(decimal amount);
		Task<bool> ProcessPaymentFromQRCodeAsync(string qrCode);
	}
}