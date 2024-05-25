namespace Fiap.McTech.Application.Dtos.Payments
{
    /// <summary>
    /// Represents the output data for a payment operation.
    /// </summary>
	public class PaymentOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentOutputDto"/> class.
        /// </summary>
        /// <param name="success">Indicates whether the payment operation was successful.</param>
        /// <param name="message">A message associated with the payment operation.</param>
        public PaymentOutputDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the payment operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message associated with the payment operation.
        /// </summary>
        public string Message { get; set; }
    }
}
