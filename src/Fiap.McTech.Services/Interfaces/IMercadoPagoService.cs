namespace Fiap.McTech.Infra.Services.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<string> GeneratePaymentLinkAsync(decimal amount);
        Task<bool> ProcessPaymentAsync(Guid paymentId);
    }
}
