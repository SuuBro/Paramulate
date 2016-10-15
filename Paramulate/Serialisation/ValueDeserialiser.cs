using System;
using Newtonsoft.Json;

namespace Paramulate.Serialisation
{
    public class InvalidProvidedValueException : Exception
    {
        public InvalidProvidedValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class ValueDeserialiser
    {
        public static object GetValue(string inValue, Type targetType, string targetName)
        {
            if (RequiresQuoting(inValue, targetType))
            {
                inValue = string.Format("{0}{1}{0}", "'", inValue);
            }

            var deserialised = JsonConvert.DeserializeObject(inValue);
            try
            {
                return Convert.ChangeType(deserialised, targetType);
            }
            catch (InvalidCastException e)
            {
                throw new InvalidProvidedValueException(
                    $"Failed to convert value '{inValue}' for property '{targetName}' (type:{targetType})", e);
            }

        }

        private static bool RequiresQuoting(string inValue, Type targetType)
        {
            return (targetType == typeof(string) || targetType == typeof(DateTime))
                   && inValue.Length > 0 && inValue[0] != '\'';
        }
    }
}