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

			if(qrCodeUrl == null)
				return BadRequest(new { Message = "Error to generate Qr Code" });

			return Ok(new { QRCodeUrl = qrCodeUrl });
		}

		[HttpPost("Pay/{paymentId}")]
		public async Task<IActionResult> Pay([FromRoute] Guid paymentId, [FromBody] string qrCode)
		{
			var paymentResult = await _paymentAppService.PayAsync(paymentId, qrCode);
			if (!paymentResult.Success)
				return BadRequest(new { Message = paymentResult.Message });

			return Ok(paymentResult);
		}
	}
}