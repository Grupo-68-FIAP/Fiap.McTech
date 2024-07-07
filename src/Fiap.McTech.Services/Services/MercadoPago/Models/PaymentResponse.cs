using System;
using System.Text.Json.Serialization;

namespace Fiap.McTech.Services.Services.MercadoPago.Models
{
    public class PaymentResponse
    {
        public PointOfInteraction PointOfInteraction { get; set; }
    }

    public class PointOfInteraction
    {
        public TransactionData TransactionData { get; set; }
    }

    public class TransactionData
    {
        [JsonPropertyName("ticket_url")]
        public string TicketUrl { get; set; }

        [JsonPropertyName("qr_code_base64")]
        public string QrCodeBase64 { get; set; }
    }
}
