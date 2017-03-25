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
            var mock = new MockIValueProvider(InitResult.UnrecognisedArgs(new []
            {
                new Value("key2", "11", "Command Line"),
            }));
            Assert.That(() => new ParamsBuilder<ITestParameterObject>(new []{mock}),
                Throws.InstanceOf<UnrecognisedValueKeyException>()
                .With.Message.Contains("Value was provided for key 'key2' which is not a valid key for " +
                                       "the Paramulate tree with root type ITestParameterObject. "+
                                       "Unrecognised Key 'key2' had value '11' from 'Command Line'. "));

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
            throw new System.NotImplementedException();
        }
    }
}