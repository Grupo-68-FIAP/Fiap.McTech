using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Entities.Payments
{
    /// <summary>
    /// Represents a payment in the system.
    /// </summary>
    public class Payment : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class with specified parameters.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client (optional).</param>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <param name="value">The value of the payment.</param>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="clientEmail">The email address of the client.</param>
        /// <param name="method">The payment method used.</param>
        /// <param name="status">The status of the payment.</param>
        /// <param name="notes">Any additional notes regarding the payment (optional).</param>
        /// <param name="discount">The discount applied to the payment (optional).</param>
        /// <param name="additionalFees">Any additional fees included in the payment (optional).</param>
        public Payment(
            Guid? clientId,
            Guid orderId,
            decimal value,
            string clientName,
            string clientEmail,
            PaymentMethod method,
            PaymentStatus status,
            string notes = "",
            decimal discount = 0,
            decimal additionalFees = 0)
        {
            ClientId = clientId;
            OrderId = orderId;
            Value = value;
            ClientName = clientName;
            ClientEmail = clientEmail;
            Method = method;
            Status = status;
            Notes = notes;
            Discount = discount;
            AdditionalFees = additionalFees;
        }

        /// <summary>
        /// Gets the unique identifier of the client (optional).
        /// </summary>
        public Guid? ClientId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the order.
        /// </summary>
        public Guid OrderId { get; private set; }

        /// <summary>
        /// Gets the value of the payment.
        /// </summary>
        public decimal Value { get; private set; } = 0;

        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        public string ClientName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the email address of the client.
        /// </summary>
        public string ClientEmail { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the payment method used.
        /// </summary>
        public PaymentMethod Method { get; private set; } = PaymentMethod.None;

        /// <summary>
        /// Gets the status of the payment.
        /// </summary>
        public PaymentStatus Status { get; private set; } = PaymentStatus.None;

        /// <summary>
        /// Gets any additional notes regarding the payment (optional).
        /// </summary>
        public string Notes { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the discount applied to the payment (optional).
        /// </summary>
        public decimal Discount { get; private set; } = 0;

        /// <summary>
        /// Gets any additional fees included in the payment (optional).
        /// </summary>
        public decimal AdditionalFees { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the billing name associated with the payment.
        /// </summary>
        public string BillingName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the billing CNPJ associated with the payment.
        /// </summary>
        public string BillingCnpj { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the billing address associated with the payment.
        /// </summary>
        public string BillingAddress { get; private set; } = string.Empty;

        /// <summary>
        /// Updates the status of the payment.
        /// </summary>
        /// <param name="status">The new status of the payment.</param>
        public void UpdateStatus(PaymentStatus status)
        {
            Status = status;
            UpdatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether the payment is valid.
        /// </summary>
        /// <returns>True if the payment is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return OrderId != Guid.Empty &&
                   Value > 0 &&
                   !string.IsNullOrWhiteSpace(ClientName) &&
                   !string.IsNullOrWhiteSpace(ClientEmail);
        }
    }
}
