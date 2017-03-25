﻿using System.Collections.Generic;

namespace Paramulate.ValueProviders
{
    /// <summary>
    /// This class is returned by an IValueProvider's Init method, and is used to indicate the results
    /// of the Init. This includes unrecognised keys for which the value provider has values for, which
    /// could indicate an error in the input keys.
    /// </summary>
    public sealed class InitResult
    {
        private InitResult(IList<Value> unrecognisedSourceValues)
        {
            UnrecognisedSourceValues = unrecognisedSourceValues;
        }

        /// <summary>
        /// The unrecognised source values
        /// </summary>
        public IList<Value> UnrecognisedSourceValues { get; }

        public static InitResult Ok()
        {
            return new InitResult(new Value[0]);
        }

        public static InitResult UnrecognisedArgs(IList<Value> unrecognisedSourceValues)
        {
            return new InitResult(unrecognisedSourceValues);
        }
    }
}