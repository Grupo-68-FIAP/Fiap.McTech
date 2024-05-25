using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Payments
{
    /// <summary>
    /// Controller responsible for handling operations related to payments.
    /// </summary>
    [Route("api/payment")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class PaymentController : Controller
    {
        private readonly IPaymentAppService _paymentAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class with the specified payment application service.
        /// </summary>
        /// <param name="paymentAppService">The service to manage payment operations.</param>
        public PaymentController(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        /// <summary>
        /// Generates a QR code for the specified order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order for which to generate the QR code.</param>
        /// <returns>The URL of the generated QR code.</returns>
        /// <response code="200">Returns the URL of the generated QR code.</response>
        /// <response code="400">Indicates that there was an error generating the QR code.</response>
        [HttpGet("GenerateQRCode/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateQRCode([FromRoute] Guid orderId)
        {
            var qrCodeUrl = await _paymentAppService.GenerateQRCodeAsync(orderId);

            if (qrCodeUrl == null)
                return BadRequest(new { Message = "Error to generate Qr Code" });

            return Ok(new { QRCodeUrl = qrCodeUrl });
        }

        /// <summary>
        /// Processes a payment using the specified payment ID and QR code.
        /// </summary>
        /// <param name="paymentId">The unique identifier of the payment to be processed.</param>
        /// <param name="qrCode">The QR code associated with the payment.</param>
        /// <returns>The result of the payment processing.</returns>
        /// <response code="200">Returns the result of the payment processing.</response>
        /// <response code="400">Indicates that there was an error processing the payment.</response>
        [HttpPost("{paymentId}/checkout")]
        [ProducesResponseType(typeof(PaymentOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pay([FromRoute] Guid paymentId, [FromBody] string qrCode)
        {
            var paymentResult = await _paymentAppService.PayAsync(paymentId, qrCode);
            if (!paymentResult.Success)
                return BadRequest(new { message = paymentResult.Message });

            return Ok(paymentResult);
        }
    }
}
