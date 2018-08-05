using System;
using System.Reflection;

namespace Paramulate.Exceptions
{
    public sealed class InvalidPropertySpecifierException : ParamulateException
    {
        internal InvalidPropertySpecifierException(Type containingType, string propertyName, OverrideDetails details)
            :this(containingType, propertyName, details.Attribute.Path, details.Property, details.ContainingType)
        {
        }

        internal InvalidPropertySpecifierException(Type containingType, string propertyName, string specifier,
            PropertyInfo onProperty, Type inType)
            :base($"Could not find the property '{propertyName}' in the type '{containingType}'. Please chack the specifier" +
                  $" '{specifier}' on property '{onProperty.Name}' in the type '{inType}'")
        {
        }
    }
}