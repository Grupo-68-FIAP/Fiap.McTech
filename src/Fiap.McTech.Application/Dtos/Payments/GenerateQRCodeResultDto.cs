using System;

namespace Fiap.McTech.Application.Dtos.Payments
{
	public class GenerateQRCodeResultDto
	{
		public GenerateQRCodeResultDto(bool success, string message, Guid? paymentId, string qrCode)
		{
			Success = success;
			Message = message;
			PaymentId = paymentId;
			QRCode = qrCode;
		}

		public bool Success { get; set; }
		public string Message { get; set; }
        public Guid? PaymentId { get; set; }
        public string QRCode { get; set; }
	}
}