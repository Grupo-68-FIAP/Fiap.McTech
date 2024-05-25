namespace Fiap.McTech.Application.Dtos.Message;

/// <summary>
/// Represents a message DTO used for conveying success status and messages.
/// </summary>
public class MessageDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDto"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation is successful.</param>
    /// <param name="message">The message associated with the operation.</param>
    public MessageDto(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the operation is successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the operation.
    /// </summary>
    public string Message { get; set; }
}
