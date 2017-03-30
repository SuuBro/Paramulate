namespace Paramulate.ValueProviders
{
    public struct UnrecognisedParameter
    {
        public string Parameter { get; }
        public string SourceHint { get; }

        public UnrecognisedParameter(string parameter, string sourceHint)
        {
            Parameter = parameter;
            SourceHint = sourceHint;
        }

        public override string ToString()
        {
            return $"{{ {nameof(Parameter)}: {Parameter}," +
                   $" {nameof(SourceHint)}: {SourceHint} }}";
        }
    }
}