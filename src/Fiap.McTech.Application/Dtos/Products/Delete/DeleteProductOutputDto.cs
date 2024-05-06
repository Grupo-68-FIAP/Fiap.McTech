using System;

namespace Fiap.McTech.Application.Dtos.Products.Delete
{
	public class DeleteProductOutputDto
	{
		public DeleteProductOutputDto(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; set; }
		public string Message { get; set; }
	}
}
