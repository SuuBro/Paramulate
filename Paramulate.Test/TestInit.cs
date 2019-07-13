using System;
using NUnit.Framework;
using Paramulate.Exceptions;
using Paramulate.ValueProviders;

namespace Paramulate.Test
{
    [TestFixture]
    public class TestInit
    {
        [Test]
        public void TestUnexpectedValueKeyPresent()
        {
            var mock = new MockIValueProvider(InitResult.UnrecognisedParams(new []
            {
                new UnrecognisedParameter("--key2=11", "Command Line"),
                new UnrecognisedParameter("--key3", "Command Line"),
                new UnrecognisedParameter("13", "Command Line"),
            }));
            Assert.That(() => new ParamsBuilder<ITestParameterObject>("", new []{mock}, true),
                Throws.InstanceOf<UnrecognisedParameterException>()
                .With.Message.Contains("Unrecognised arguments were provided which can not be set on " +
                                       "the Paramulate tree with root type Paramulate.Test.ITestParameterObject. "+
                                       $"Unrecognised arguments:{Environment.NewLine}" +
                                       $"  '--key2=11' from Command Line{Environment.NewLine}" +
                                       $"  '--key3' from Command Line{Environment.NewLine}" +
                                       $"  '13' from Command Line"));

        }
    }

    public class MockIValueProvider : IValueProvider
    {
        private readonly InitResult _result;

        public MockIValueProvider(InitResult result)
        {
            _result = result;
        }

        public InitResult Init(KeyData[] knownKeys)
        {
            return _result;
        }

        public Value? GetValue(string key)
        {
            throw new NotSupportedException();
        }
    }
}