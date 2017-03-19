using System.Collections.Generic;

namespace Paramulate.ValueProviders
{
    public sealed class InitResult
    {
        public InitResult(IList<KeyData> unrecognisedSourceValues)
        {
            UnrecognisedSourceValues = unrecognisedSourceValues;
        }

        public IList<KeyData> UnrecognisedSourceValues { get; }
    }
}