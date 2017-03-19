using System;

namespace Paramulate.Exceptions
{
    public sealed class InvalidProvidedValueException : ParamulateException
    {
        internal InvalidProvidedValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}