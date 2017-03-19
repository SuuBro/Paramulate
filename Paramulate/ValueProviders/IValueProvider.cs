namespace Paramulate.ValueProviders
{
    /// <summary>
    /// Implement this interface to provide values for you Paramulate objects
    ///  </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// This will be called on your value provider for a particular paramulate tree, with the known keys
        /// of that tree. You should fill in the init result object and retuern it so the framework can provide
        /// feedback to the user if required (E.g. if values provided to your ValueProvider do not match any possible
        /// keys in the paraumulate tree the framework may warn the user they have provided unrecognised parameters.
        /// </summary>
        /// <param name="knownKeys">A list of parameters for a given paramulate tree</param>
        /// <returns>Results of the initilisation, including unrecognised values</returns>
        InitResult Init(KeyData[] knownKeys);

        /// <summary>
        /// Get a value for a key
        /// </summary>
        /// <param name="key">Key for which a value will be returned, e.g. RootName.NestedObj.PropertyName</param>
        /// <returns>A value for the specified key which will be set on the Paramulate object</returns>
        Value? GetValue(string key);
    }
}