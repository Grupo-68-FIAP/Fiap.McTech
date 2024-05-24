using System;

namespace Fiap.McTech.Application.Dtos.Orders.Delete
{
    /// <summary>
    /// Represents the output data for deleting an order.
    /// </summary>
	public class DeleteOrderOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteOrderOutputDto"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation is successful.</param>
        /// <param name="message">The message associated with the operation.</param>
        public DeleteOrderOutputDto(bool isSuccess, string message)
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
}