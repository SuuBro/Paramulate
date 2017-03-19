namespace Paramulate.ValueProviders
{
    public struct Value
    {
        public readonly string ValueToSet;
        public readonly string SourceHint;

        public Value(string value, string sourceHint)
        {
            ValueToSet = value;
            SourceHint = sourceHint;
        }
    }


    /// <summary>
    /// Implement this interface to provide values for you Paramulate objects
    ///  </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Get a value for a key
        /// </summary>
        /// <param name="key">Key for which a value will be returned, e.g. RootName.NestedObj.PropertyName</param>
        /// <returns>A value for the specified key which will be set on the Paramulate object</returns>
        Value? GetValue(string key);
    }
}