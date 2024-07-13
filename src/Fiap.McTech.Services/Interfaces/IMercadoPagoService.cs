using Fiap.McTech.Services.Services.MercadoPago.Models;

namespace Fiap.McTech.Infra.Services.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<string> GeneratePaymentLinkAsync(PaymentRequest request);
        Task<bool> ProcessPaymentAsync(Guid paymentId);
    }
}
