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
                new UnrecognisedParameter("--key2=Value2", "Command Line"),
            }));

        }
    }
}