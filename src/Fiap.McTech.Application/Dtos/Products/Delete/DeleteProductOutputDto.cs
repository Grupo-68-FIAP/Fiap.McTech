namespace Fiap.McTech.Application.Dtos.Products.Delete
{
    /// <summary>
    /// Represents the output data for deleting a product.
    /// </summary>
	public class DeleteProductOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProductOutputDto"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the deletion operation was successful.</param>
        /// <param name="message">A message associated with the deletion operation.</param>
        public DeleteProductOutputDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the deletion operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a message associated with the deletion operation.
        /// </summary>
        public string Message { get; set; }
    }
}
