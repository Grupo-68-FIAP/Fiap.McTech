namespace Fiap.McTech.Application.Dtos.Message;

public class MessageDto
{
    public MessageDto(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; set; }
		public string Message { get; set; }
}