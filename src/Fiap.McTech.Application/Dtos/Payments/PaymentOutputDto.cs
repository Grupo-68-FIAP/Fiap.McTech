using System;

namespace Fiap.McTech.Application.Dtos.Payments
{
	public class PaymentOutputDto
	{
		public PaymentOutputDto(bool success, string message)
		{
			Success = success;
			Message = message;
		}

		public bool Success { get; set; }
		public string Message { get; set; }
	}
}