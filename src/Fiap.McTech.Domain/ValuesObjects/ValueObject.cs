using System;

namespace Fiap.McTech.Domain.ValuesObjects
{
    /// <summary>
    /// Represents a base class for value objects.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Determines whether the value object is valid.
        /// </summary>
        /// <returns>True if the value object is valid, otherwise false.</returns>
        public abstract bool IsValid();
    }
}
