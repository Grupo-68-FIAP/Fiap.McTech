﻿namespace Fiap.McTech.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when payment is required in the McTech domain.
    /// </summary>
    public class PaymentRequiredException : McTechException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PaymentRequiredException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRequiredException"/> class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference 
        /// if no inner exception is specified.</param>
        public PaymentRequiredException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
