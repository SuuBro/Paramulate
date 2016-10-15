using NUnit.Framework;
using Paramulate.Attributes;

namespace Comparams.Test
{
    internal class TestData
    {
        public const int TestInt = 23;
        public const string TestIntStr = "23";
    }

    public interface ITestParameterObject
    {
        [Default(TestData.TestIntStr)]
        int TestParameter1 { get; }
    }

    [TestFixture]
    public class TestDefaults
    {
        [Test]
        public void TestSimple()
        {
            var builder = ParamsBuilder<ITestParameterObject>.New();
            var result = builder.Build();
            Assert.That(result.TestParameter1, Is.EqualTo(TestData.TestInt));
        }
    }
}