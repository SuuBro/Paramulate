using NUnit.Framework;
using Paramulate.Attributes;
using Paramulate.ValueProviders;

namespace Paramulate.Test
{
    public interface IInterface
    {
        [CommandLine("shortName", "Set the TestInt property")]
        int TestInt { get; }

        [CommandLine("s", "shorterName", "Set the TestInt property")]
        int TestIntShort { get; }
        
        [CommandLine("TestString", "ds", "deepString", "Set the OneLevel.TestString porperty")]
        IDeeper OneLevel { get; }
    }

    public interface IDeeper
    {
        string TestString { get; }
    }

    [TestFixture]
    public class TestCommandlineValueProvider
    {
        [Test]
        public void TestUnexpectedValueKeyPresent()
        {
            var uut = new CommandLineValueProvider(new []{"--key1=Value1", "--key2=Value2"});
            var knownKeys = new[]
            {
                new KeyData(typeof(string), "Object.Nested.Key1", new CommandLineKeys("key1", "k"))
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
            uut.Init(new []{ new KeyData(typeof(string), key) });
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

        [Test]
        public void TestCommandLineTrumpsDefault()
        {
            const int value = 2;
            var uut = new CommandLineValueProvider(new []{$"--Root.Level3Int={value}"});
            var builder = new ParamsBuilder<ILevel3Params>("Root", new []{uut}, true);
            var result = builder.Build();
            Assert.That(result.Level3Int, Is.EqualTo(value));
        }

        [Test]
        [TestCase("shortName")]
        [TestCase("Root.TestInt")]
        public void TestCommandLineCanUseReferenceKey(string key)
        {
            const int value = 2;
            var uut = new CommandLineValueProvider(new []{$"--{key}={value}"});
            var builder = new ParamsBuilder<IInterface>("Root", new []{uut}, true);
            var result = builder.Build();
            Assert.That(result.TestInt, Is.EqualTo(value));
        }

        [Test]
        [TestCase("s")]
        [TestCase("shorterName")]
        [TestCase("Root.TestIntShort")]
        public void TestCommandLineCanUseShortReferenceKey(string key)
        {
            const int value = 2;
            var uut = new CommandLineValueProvider(new []{$"-{key}={value}"});
            var builder = new ParamsBuilder<IInterface>("Root", new []{uut}, true);
            var result = builder.Build();
            Assert.That(result.TestIntShort, Is.EqualTo(value));
        }
        
        [Test]
        [TestCase("ds")]
        [TestCase("deepString")]
        [TestCase("Root.OneLevel.TestString")]
        public void TestCommandLineWithDeeperProperty(string key)
        {
            const string value = "HelloMate";
            var uut = new CommandLineValueProvider(new []{$"--{key}={value}"});
            var builder = new ParamsBuilder<IInterface>("Root", new []{uut}, true);
            var result = builder.Build();
            Assert.That(result.OneLevel.TestString, Is.EqualTo(value));
        }
    }
}