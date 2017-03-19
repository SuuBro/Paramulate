namespace Paramulate.ValueProviders
{
    public struct Value
    {
        public string ValueToSet { get; }
        public string SourceHint { get; }

        public Value(string value, string sourceHint)
        {
            ValueToSet = value;
            SourceHint = sourceHint;
        }
    }
}