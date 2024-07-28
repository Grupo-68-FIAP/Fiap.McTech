using Bogus;
using Fiap.McTech.Services.Services.MercadoPago.Models;

namespace Fiap.McTech.UnitTests.Fakers
{
    public static class PaymentServiceFaker
    {
        public static PaymentRequest GeneratePaymentRequest()
        {
            var faker = new Faker<PaymentRequest>()
                .RuleFor(pr => pr.TransactionAmount, f => f.Finance.Amount(50, 500))
                .RuleFor(pr => pr.Description, f => f.Lorem.Sentence())
                .RuleFor(pr => pr.PaymentMethodId, f => "pix")
                .RuleFor(pr => pr.Payer, f => new Payer
                {
                    Email = f.Internet.Email(),
                    FirstName = f.Name.FirstName(),
                    LastName = f.Name.LastName(),
                    Identification = new Identification
                    {
                        Type = "CPF",
                        Number = f.Random.Replace("###########") // Gera um número de CPF falso
                    }
                });

            return faker.Generate();
        }

        public static PaymentResponse GeneratePaymentResponse()
        {
            var faker = new Faker<PaymentResponse>()
                .RuleFor(pr => pr.PointOfInteraction, f => new PointOfInteraction
                {
                    TransactionData = new TransactionData
                    {
                        TicketUrl = f.Internet.Url(),
                        QrCodeBase64 = Convert.ToBase64String(f.Random.Bytes(20))
                    }
                });

            return faker.Generate();
        }
    }
}
