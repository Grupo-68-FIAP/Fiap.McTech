using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.McTech.Domain.Exceptions
{
    public class PaymentRequiredException : McTechException
    {
        public PaymentRequiredException() { }

        public PaymentRequiredException(string message)
            : base(message) { }

        public PaymentRequiredException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
