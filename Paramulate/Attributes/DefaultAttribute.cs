using System;

namespace Paramulate.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DefaultAttribute : Attribute
    {
        public string Value { get; }

        public DefaultAttribute(string value)
        {
            Value = value;
        }
    }
}