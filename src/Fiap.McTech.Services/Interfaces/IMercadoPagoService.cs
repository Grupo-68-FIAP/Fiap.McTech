namespace Fiap.McTech.Infra.Services.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<string> GeneratePaymentLinkAsync(decimal amount);
        Task<bool> ProcessPaymentFromQRCodeAsync(string qrCode);
    }
}
