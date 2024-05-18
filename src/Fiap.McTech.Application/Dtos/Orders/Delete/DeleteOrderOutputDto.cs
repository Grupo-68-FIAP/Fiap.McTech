using System;

namespace Fiap.McTech.Application.Dtos.Orders.Delete
{
	public class DeleteOrderOutputDto
	{
		public DeleteOrderOutputDto(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; set; }
		public string Message { get; set; }
	}
}