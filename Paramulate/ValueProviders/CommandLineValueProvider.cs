using System.Collections.Generic;
using System.Linq;
using Mono.Options;

namespace Paramulate.ValueProviders
{
    public sealed class CommandLineValueProvider : IValueProvider
    {
        internal static string Hint => "Command Line";

        private readonly string[] _arguments;

        private readonly Dictionary<string,string> _values = new Dictionary<string, string>();

        public CommandLineValueProvider(string[] arguments)
        {
            _arguments = arguments;
        }

        public InitResult Init(KeyData[] knownKeys)
        {
            var helpRequested = false;
            new OptionSet { {"h|?|help", v => helpRequested = true} }.Parse(_arguments);
            if (helpRequested)
            {
                return InitResult.HelpRequested();
            }
            
            var optionSet = MakeOptionSet(knownKeys);
            var unknownArgs = optionSet.Parse(_arguments);

            return unknownArgs.Any()
                ? InitResult.UnrecognisedParams(unknownArgs.Select(a => new UnrecognisedParameter(a, Hint)).ToList())
                : InitResult.Ok();
        }

        private OptionSet MakeOptionSet(IEnumerable<KeyData> knownKeys)
        {
            var set = new OptionSet();
            foreach (var key in knownKeys)
            {
                set.Add(KeyToOption(key), v => _values[key.FullKey] = v);
            }
            return set;
        }

        private static string KeyToOption(KeyData key)
        {
            var options = new []{ key.FullKey }
                .Concat(key.CommandLineKeys.Select(k => k.Alias))
                .Concat(key.CommandLineKeys.Select(k => k.ShortAlias))
                .Where(o => o != null)
                .Distinct();

            var optionString = key.Type == typeof(bool)
                ? options
                : options.Select(o => o + "=");

            return string.Join("|", optionString);
        }

        public Value? GetValue(string key)
        {
            string value;
            return _values.TryGetValue(key, out value)
                ? (Value?) new Value(key, value, Hint)
                : null;
        }
    }
}