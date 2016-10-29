using System;

namespace Paramulate.Exceptions
{
    public class InvalidProvidedValueException : Exception
    {
        internal InvalidProvidedValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}