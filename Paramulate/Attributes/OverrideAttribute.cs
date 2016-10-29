using System;

namespace Paramulate.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class OverrideAttribute : Attribute
    {
        public string Path { get; }

        public string Value { get; }

        public OverrideAttribute(string pathToProperty, string value)
        {
            Path = pathToProperty;
            Value = value;
        }
    }
}