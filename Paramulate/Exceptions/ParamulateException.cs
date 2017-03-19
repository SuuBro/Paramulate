using System;

namespace Paramulate.Exceptions
{
    /// <summary>
    /// Allow framework consumers to catch any Paramulate exception
    /// </summary>
    public class ParamulateException : Exception
    {
        public ParamulateException()
        {
        }

        public ParamulateException(string message) : base(message)
        {
        }

        public ParamulateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}