using System;
using System.Text.Json.Serialization;

namespace Fiap.McTech.Services.Services.MercadoPago.Models
{
    public class PaymentRequest
    {
        [JsonPropertyName("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("payment_method_id")]
        public string PaymentMethodId { get; set; }

        [JsonPropertyName("payer")]
        public Payer Payer { get; set; }
    }

    public class Payer
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("identification")]
        public Identification Identification { get; set; }
    }

    public class Identification
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }
    }
}
