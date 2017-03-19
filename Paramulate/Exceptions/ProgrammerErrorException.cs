using System;

namespace Paramulate.Exceptions
{
    public sealed class ProgrammerErrorException : ParamulateException
    {
        internal ProgrammerErrorException(string reason)
            : base($"{reason} {Environment.NewLine}" +
                   $" Please raise an issue via GitHub")
        {
        }
    }
}