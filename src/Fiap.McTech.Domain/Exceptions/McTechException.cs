using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.McTech.Domain.Exceptions
{
    public abstract class McTechException : Exception
    {
        public McTechException() { }

        public McTechException(string message)
            : base(message) { }

        public McTechException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
