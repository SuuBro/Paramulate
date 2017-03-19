namespace Paramulate.ValueProviders
{
    public sealed class KeyData
    {
        internal KeyData(string fullKey, string referenceKey, string shortReferenceKey)
        {
            FullKey = fullKey;
            ReferenceKey = referenceKey;
            ShortReferenceKey = shortReferenceKey;
        }

        public string FullKey { get; }

        public string ReferenceKey { get; }

        public string ShortReferenceKey { get; }
    }
}