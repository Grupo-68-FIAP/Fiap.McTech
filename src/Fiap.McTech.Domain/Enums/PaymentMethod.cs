using System;

namespace Fiap.McTech.Domain.Enums
{
    public enum PaymentMethod
    {
        None = -1,
        CreditCard,
        DebitCard,
        Pix,
        QrCode
    }
}