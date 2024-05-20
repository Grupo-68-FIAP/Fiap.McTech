using Fiap.McTech.Application.Dtos.Payments;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IPaymentAppService
	{
		Task<GenerateQRCodeResultDto> GenerateQRCodeAsync(Guid orderId);
		Task<PaymentOutputDto> PayAsync(Guid paymentId, string qrCode);
	}
}