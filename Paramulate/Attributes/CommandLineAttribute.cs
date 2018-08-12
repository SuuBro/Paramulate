using System;
using Paramulate.ValueProviders;

namespace Paramulate.Attributes
{
    /// <summary>
    /// Place these on properties of the top-level type in your parameter
    /// tree (i.e. the type you pass to the builder) only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AliasAttribute : Attribute
    {
        public string PathToDeeperKey { get; }
        public string ShortAlias { get; }
        public string Alias { get; }
        public string HelpText { get; }

        public AliasAttribute(string helpText)
        {
            HelpText = helpText;
        }

        public AliasAttribute(string alias, string helpText)
            : this(helpText)
        {
            Alias = alias;
        }

        public AliasAttribute(string shortAlias, string alias, string helpText)
            : this(alias, helpText)
        {
            ShortAlias = shortAlias;
        }

        public AliasAttribute(string pathToDeeperKey, string shortAlias, string alias, string helpText)
            : this(shortAlias, alias, helpText)
        {
            PathToDeeperKey = pathToDeeperKey;
        }
    }
}