using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.ViewModels.Payments;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IPaymentAppService
	{
		Task<GenerateQRCodeResultDto> GenerateQRCodeAsync(Guid orderId);
		Task<PaymentOutputDto> PayAsync(Guid orderId, string qrCode);
	}
}