using System;
using System.Text.Json.Serialization;

namespace Fiap.McTech.Services.Services.MercadoPago.Models
{
    public class PaymentResponse
    {
        [JsonPropertyName("point_of_interaction")]
        public PointOfInteraction? PointOfInteraction { get; set; }
    }

    public class PointOfInteraction
    {
        [JsonPropertyName("transaction_data")]
        public TransactionData? TransactionData { get; set; }
    }

    public class TransactionData
    {
        [JsonPropertyName("ticket_url")]
        public string TicketUrl { get; set; } = string.Empty;

        [JsonPropertyName("qr_code")]
        public string QrCodeBase64 { get; set; } = string.Empty;
    }
}
