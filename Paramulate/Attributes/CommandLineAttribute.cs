using System;

namespace Paramulate.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CommandLineAttribute : Attribute
    {
        public string PathToDeeperKey { get; }
        public string ShortReferenceKey { get; }
        public string ReferenceKey { get; }
        public string HelpText { get; }

        public CommandLineAttribute(string helpText)
        {
            HelpText = helpText;
        }

        public CommandLineAttribute(string referenceKey, string helpText)
            : this(helpText)
        {
            ReferenceKey = referenceKey;
        }

        public CommandLineAttribute(string shortReferenceKey, string referenceKey, string helpText)
            : this(referenceKey, helpText)
        {
            ShortReferenceKey = shortReferenceKey;
        }

        public CommandLineAttribute(string pathToDeeperKey, string shortReferenceKey,
            string referenceKey, string helpText)
            : this(shortReferenceKey, referenceKey, helpText)
        {
            throw new NotImplementedException("Yet!");
            PathToDeeperKey = pathToDeeperKey;
        }
    }
}