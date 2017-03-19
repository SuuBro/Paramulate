namespace Paramulate.ValueProviders
{
    public struct Value
    {
        public string Key { get; }
        public string ValueToSet { get; }
        public string SourceHint { get; }

        public Value(string key, string value, string sourceHint)
        {
            Key = key;
            ValueToSet = value;
            SourceHint = sourceHint;
        }
    }
}