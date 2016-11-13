using System;
using Newtonsoft.Json;
using Paramulate.Exceptions;

namespace Paramulate.Serialisation
{
    internal class ValueSerialser
    {
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings {Formatting = Formatting.Indented});
        }
    }

    internal class ValueDeserialiser
    {
        public static object GetValue(string inValue, Type targetType, string targetName, string context)
        {
            if (inValue == "null")
            {
                return null;
            }

            if (targetType == typeof(TimeSpan))
            {
                try
                {
                    return TimeSpan.Parse(inValue);
                }
                catch (FormatException e)
                {
                    throw new InvalidProvidedValueException(
                        $"Failed to convert value '{inValue}' for property '{targetName}' (type:{targetType}) when" +
                        $" {context}", e);
                }
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = targetType.GenericTypeArguments[0];
            }

            if (RequiresQuoting(inValue, targetType))
            {
                inValue = inValue.Replace("'", "\\'");
                inValue = $"\'{inValue}\'";
            }

            object deserialised;
            try
            {
                deserialised = JsonConvert.DeserializeObject(inValue);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidProvidedValueException(
                    $"Failed to deserialize value '{inValue}' for property '{targetName}' (type:{targetType})" +
                    $" when {context}", e);
            }

            try
            {
                return Convert.ChangeType(deserialised, targetType);
            }
            catch (InvalidCastException e)
            {
                throw new InvalidProvidedValueException(
                    $"Failed to convert value '{inValue}' for property '{targetName}' (type:{targetType})" +
                    $" when {context}", e);
            }

        }

        private static bool RequiresQuoting(string inValue, Type targetType)
        {
            return (inValue.Length > 0 && inValue[0] != '\'') &&
                   (targetType == typeof(string)
                 || targetType == typeof(DateTime));
        }
    }
}