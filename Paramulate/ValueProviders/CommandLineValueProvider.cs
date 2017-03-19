using System;

namespace Paramulate.ValueProviders
{
    public sealed class CommandLineValueProvider : IValueProvider
    {
        public CommandLineValueProvider(string[] arguments)
        {
            throw new NotImplementedException();
        }

        public InitResult Init(KeyData[] knownKeys)
        {
            throw new NotImplementedException();
        }

        public Value? GetValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}