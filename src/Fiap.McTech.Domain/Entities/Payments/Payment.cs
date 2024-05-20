using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Entities.Payments
{
    public class Payment : EntityBase
	{
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

		public Guid? ClientId { get; private set; }
        public Guid OrderId { get; private set; }
		public decimal Value { get; private set; } = 0;
		public string ClientName { get; private set; } = string.Empty;
		public string ClientEmail { get; private set; } = string.Empty;
		public PaymentMethod Method { get; private set; } = PaymentMethod.None;
		public PaymentStatus Status { get; private set; } = PaymentStatus.None;
		public string Notes { get; private set; } = string.Empty;
		public decimal Discount { get; private set; } = 0;
		public decimal AdditionalFees { get; private set; } = 0;
		public string BillingName { get; private set; } = string.Empty;
		public string BillingCnpj { get; private set; } = string.Empty;
		public string BillingAddress { get; private set; } = string.Empty;

		public void UpdateStatus(PaymentStatus status)
		{
			Status = status;
			UpdatedDate = DateTime.UtcNow;
		}

		public override bool IsValid()
		{
			return OrderId != Guid.Empty &&
				   Value > 0 &&
				   !string.IsNullOrWhiteSpace(ClientName) &&
				   !string.IsNullOrWhiteSpace(ClientEmail);
		}
	}
}