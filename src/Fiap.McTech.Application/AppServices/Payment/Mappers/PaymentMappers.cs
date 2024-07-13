using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Services.Services.MercadoPago.Models;

namespace Fiap.McTech.Application.AppServices.Payment.Mappers
{
    public static class PaymentMappers
    {
        public static PaymentRequest MapPaymentToServiceModel(this Order order)
        {
            var request = new PaymentRequest()
            {
                TransactionAmount = order.TotalAmount,
                Description = string.Empty,
                PaymentMethodId = "qrCode",
                Payer = new Payer
                {
                    Email = order.Client.Email.ToString(),
                    FirstName = order.Client.Name,
                    LastName = order.Client.Name,
                    Identification = new Identification
                    {
                        Type = "CPF",
                        Number = order.Client.Cpf.ToString()
                    }
                }
            };

            return request;
        }
    }
}
