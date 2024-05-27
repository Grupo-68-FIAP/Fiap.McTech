namespace Fiap.McTech.Application.Dtos.Payments
{
    /// <summary>
    /// Represents the result of generating a QR code for a payment.
    /// </summary>
	public class GenerateQRCodeResultDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateQRCodeResultDto"/> class.
        /// </summary>
        /// <param name="success">Indicates whether the generation of the QR code was successful.</param>
        /// <param name="message">A message associated with the result.</param>
        /// <param name="paymentId">The ID of the payment associated with the QR code.</param>
        /// <param name="qrCode">The generated QR code.</param>
        public GenerateQRCodeResultDto(bool success, string message, Guid? paymentId, string qrCode)
        {
            Success = success;
            Message = message;
            PaymentId = paymentId;
            QRCode = qrCode;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the generation of the QR code was successful.
        /// </summary>
		public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message associated with the result.
        /// </summary>
		public string Message { get; set; }

        /// <summary>
        /// Gets or sets the ID of the payment associated with the QR code.
        /// </summary>
        public Guid? PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the generated QR code.
        /// </summary>
        public string QRCode { get; set; }
    }
}
