using System;

namespace Paramulate.ValueProviders
{
    public sealed class KeyData
    {
        internal KeyData(Type type, string fullKey, string referenceKey, string shortReferenceKey)
        {
            Type = type;
            FullKey = fullKey;
            ReferenceKey = referenceKey;
            ShortReferenceKey = shortReferenceKey;
        }

        public Type Type { get; }

        public string FullKey { get; }

        public string ReferenceKey { get; }

        public string ShortReferenceKey { get; }
    }
}