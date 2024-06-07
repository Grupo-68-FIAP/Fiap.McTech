namespace Fiap.McTech.Infra.Services.Interfaces
{
    public interface IPayPalPaymentService
    {
        Task<string> GeneratePaymentLinkAsync(decimal amount);
        Task<bool> ProcessPaymentFromQRCodeAsync(string qrCode);
    }
}