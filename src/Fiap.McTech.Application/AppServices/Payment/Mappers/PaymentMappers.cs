using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Services.Services.MercadoPago.Models;

namespace Fiap.McTech.Application.AppServices.Payment.Mappers
{
    /// <summary>
    /// Classe contendo métodos de mapeamento para pagamentos.
    /// </summary>
    public static class PaymentMappers
    {
        /// <summary>
        /// Mapeia um objeto Order para um modelo de serviço PaymentRequest.
        /// </summary>
        /// <param name="order">O objeto Order a ser mapeado.</param>
        /// <returns>Um objeto PaymentRequest populado com os dados relevantes do objeto Order.</returns>
        public static PaymentRequest MapPaymentToServiceModel(this Order order)
        {
            var request = new PaymentRequest()
            {
                TransactionAmount = order.TotalAmount,
                Description = string.Empty,
                PaymentMethodId = "qrCode",
                Payer = new Payer
                {
                    Email =order?.Client?.Email.ToString() ?? string.Empty,
                    FirstName = order?.Client?.Name ?? string.Empty,
                    LastName = order?.Client?.Name ?? string.Empty,
                    Identification = new Identification
                    {
                        Type = "CPF",
                        Number = order?.Client?.Cpf.ToString() ?? string.Empty
                    }
                }
            };

            return request;
        }
    }
}
