using System;

namespace Fiap.McTech.Services.Services.MercadoPago.Models
{
    public class PaymentRequest
    {
        public decimal TransactionAmount { get; set; }
        public string Description { get; set; }
        public string PaymentMethodId { get; set; }
        public Payer Payer { get; set; }
    }

    public class Payer
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Identification Identification { get; set; }
    }

    public class Identification
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }
}
