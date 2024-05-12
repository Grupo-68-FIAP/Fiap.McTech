using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces; 

namespace Fiap.McTech.Api.Controllers.Payments
{
    public class PaymentController : Controller
	{
		public readonly IPaymentAppService _paymentAppService;

		public PaymentController(IPaymentAppService paymentAppService)
		{
			_paymentAppService = paymentAppService;
		}

		[HttpGet("GenerateQRCode/{orderId}")]
		public async Task<IActionResult> GenerateQRCode([FromRoute] Guid orderId)
		{
			var qrCodeUrl = await _paymentAppService.GenerateQRCodeAsync(orderId);

			//TODO - MELHORAR RETORNO

			return Ok(new { QRCodeUrl = qrCodeUrl });
		}

		[HttpPost("Pay/{orderId}")]
		public async Task<IActionResult> Pay([FromRoute] Guid orderId, [FromBody] string qrCode)
		{
			var paymentResult = await _paymentAppService.PayAsync(orderId, qrCode);

			//TODO - MELHORAR RETORNO

			return Ok(paymentResult);
		}
	}
}