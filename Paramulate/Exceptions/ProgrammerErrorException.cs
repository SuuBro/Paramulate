using System;

namespace Paramulate.Exceptions
{
    public class ProgrammerErrorException : Exception
    {
        internal ProgrammerErrorException(string reason)
            : base($"{reason} {Environment.NewLine}" +
                   $" Please raise an issue via GitHub")
        {
        }
    }
}