using System.Diagnostics.CodeAnalysis;

namespace Fiap.McTech.Services.Services.MercadoPago
{
    [ExcludeFromCodeCoverage]
    public class MercadoPagoConfig
    {
        public string BaseUrl { get; set; }
        public string AccessToken { get; set; }
        public string IdempotencyKey { get; set; }
    }
}
