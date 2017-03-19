using System.Collections.Generic;

namespace Paramulate.ValueProviders
{
    /// <summary>
    /// This class is returned by an IValueProvider's Init method, and is used to indicate the results
    /// of the Init. This includes unrecognised keys for which the value provider has values for, which
    /// could indicate an error in the input keys.
    /// </summary>
    public sealed class InitResult
    {
        public InitResult(IList<Value> unrecognisedSourceValues)
        {
            UnrecognisedSourceValues = unrecognisedSourceValues;
        }

        /// <summary>
        /// The unrecognised source values
        /// </summary>
        public IList<Value> UnrecognisedSourceValues { get; }
    }
}