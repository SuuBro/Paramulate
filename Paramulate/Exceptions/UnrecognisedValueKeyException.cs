namespace Paramulate.Exceptions
{
    public sealed class UnrecognisedParameterException : ParamulateException
    {
        public UnrecognisedParameterException(string message) : base(message)
        {
        }
    }
}