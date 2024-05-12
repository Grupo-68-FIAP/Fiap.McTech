using System;

namespace Fiap.McTech.Application.Dtos.Payments
{
	public class GenerateQRCodeResultDto
	{
		public GenerateQRCodeResultDto(bool success, string message, string qRCode)
		{
			Success = success;
			Message = message;
			QRCode = qRCode;
		}

		public bool Success { get; set; }
		public string Message { get; set; }
		public string QRCode { get; set; }
	}
}