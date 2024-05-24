namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the method used for payment.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// No payment method.
        /// </summary>
        None = -1,

        /// <summary>
        /// Payment made via credit card.
        /// </summary>
        CreditCard,

        /// <summary>
        /// Payment made via debit card.
        /// </summary>
        DebitCard,

        /// <summary>
        /// Payment made via Pix.
        /// </summary>
        Pix,

        /// <summary>
        /// Payment made via QR code.
        /// </summary>
        QrCode
    }
}