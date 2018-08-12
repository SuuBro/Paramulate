using System;
using System.Linq;

namespace Paramulate.ValueProviders
{
    public struct CommandLineKeys
    {
        public CommandLineKeys(string referenceKey, string shortReferenceKey)
        {
            Alias = referenceKey;
            ShortAlias = shortReferenceKey;
        }

        public string Alias { get; }

        public string ShortAlias { get; }
    }
    
    public struct KeyData
    {
        internal KeyData(Type type, string fullKey, params CommandLineKeys[] commandLineKeys)
        {
            Type = type;
            FullKey = fullKey;
            CommandLineKeys = commandLineKeys;
        }

        public Type Type { get; }

        public string FullKey { get; }

        public CommandLineKeys[] CommandLineKeys { get; }

        internal KeyData WithCommandLine(string refKey, string shortRefKey)
        {
            return new KeyData(
                Type, 
                FullKey, 
                CommandLineKeys
                    .Concat(new []{new CommandLineKeys(refKey, shortRefKey)})
                    .ToArray()
            );
        }
    }
}