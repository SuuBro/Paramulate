using System;
using System.Reflection;
using Paramulate.Attributes;

namespace Paramulate
{
    internal struct OverrideDetails
    {
        public OverrideAttribute Attribute { get; }
        public Type ContainingType { get; }
        public PropertyInfo Property { get; }

        public OverrideDetails(OverrideAttribute attribute, Type containingType, PropertyInfo property)
        {
            Attribute = attribute;
            ContainingType = containingType;
            Property = property;
        }
    }
}