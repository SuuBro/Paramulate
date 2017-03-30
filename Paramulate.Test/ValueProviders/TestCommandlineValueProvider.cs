using NUnit.Framework;
using Paramulate.ValueProviders;

namespace Paramulate.Test
{
    [TestFixture]
    public class TestCommandlineValueProvider
    {
        [Test]
        public void TestUnexpectedValueKeyPresent()
        {
            var uut = new CommandLineValueProvider(new []{"--key1=Value1", "--key2=Value2"});
            var knownKeys = new[]
            {
                new KeyData(typeof(string), "Object.Nested.Key1", "key1", "k"),
            };
            var result = uut.Init(knownKeys);
            Assert.That(result.UnrecognisedParameters, Is.EquivalentTo(new []
            {
                new UnrecognisedParameter("--key2=Value2", CommandLineValueProvider.Hint),
            }));
        }

        [Test]
        public void TestSimpleValueProvided()
        {
            const string key = "Root.Level3";
            const string value = "Value1";
            var uut = new CommandLineValueProvider(new []{$"--{key}={value}"});
            uut.Init(new []{ new KeyData(typeof(string), key, null, null) });
            var result = uut.GetValue(key);
            Assert.That(result, Is.EqualTo(new Value(key, value, CommandLineValueProvider.Hint)));
        }

        [Test]
        public void TestSimpleValueProvidedWorksWithBuilder()
        {
            const string value = "Value1";
            var uut = new CommandLineValueProvider(new []{$"--Root.Level3={value}"});
            var builder = new ParamsBuilder<ILevel3Params>("Root", new []{uut}, true);
            var result = builder.Build();
            Assert.That(result.Level3, Is.EqualTo(value));
        }
    }
}