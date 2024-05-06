using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Entities.Payments
{
    public class Payment : EntityBase
	{
        public Payment(
			Guid clientId, 
			Guid orderId, 
			decimal value, 
			string clientName,
			string clientEmail,
			PaymentMethod method,
			PaymentStatus status,
			DateTime transactionDate,
			string notes, 
			decimal discount,
			decimal additionalFees,
			string billingCnpj,
			string billingAddress)
		{
			ClientId = clientId;
			OrderId = orderId;
			Value = value;
			ClientName = clientName;
			ClientEmail = clientEmail;
			Method = method;
			Status = status;
			TransactionDate = transactionDate;
			Notes = notes;
			Discount = discount;
			AdditionalFees = additionalFees;
			BillingCnpj = billingCnpj;
			BillingAddress = billingAddress;
		}

		public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
		public decimal Value { get; private set; } = 0;
		public string ClientName { get; private set; } = string.Empty;
		public string ClientEmail { get; private set; } = string.Empty;
		public PaymentMethod Method { get; private set; } = PaymentMethod.None;
		public PaymentStatus Status { get; private set; } = PaymentStatus.None;
		public DateTime TransactionDate { get; private set; } = DateTime.UtcNow;
		public string Notes { get; private set; } = string.Empty;
		public decimal Discount { get; private set; } = 0;
		public decimal AdditionalFees { get; private set; } = 0;
		public string BillingCnpj { get; private set; } = string.Empty;
		public string BillingAddress { get; private set; } = string.Empty;

		public override bool IsValid()
		{
			return ClientId != Guid.Empty &&
				   OrderId != Guid.Empty &&
				   Value > 0 &&
				   TransactionDate != default &&
				   !string.IsNullOrWhiteSpace(ClientName) &&
				   !string.IsNullOrWhiteSpace(ClientEmail);
		}
	}
}