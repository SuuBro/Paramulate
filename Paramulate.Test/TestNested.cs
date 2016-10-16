using NUnit.Framework;
using Paramulate.Attributes;

namespace Paramulate.Test
{
    [Paramulate]
    public interface IChild
    {
        [Default("TestChild")]
        string TestChildValue { get; }
    }

    [Paramulate]
    public interface IParent
    {
        IChild Child { get; }

        [Default("TestParent")]
        string TestParentValue { get; }
    }

    [Paramulate]
    public interface IGrandParent
    {
        IParent Child { get; }

        [Default("TestGrandParent")]
        string TestGrandParentValue { get; }
    }


    [TestFixture]
    public class TestNested
    {
        [Test]
        public void TestOneLevelDeep()
        {
            var result = Build<IParent>();
            Assert.That(result.TestParentValue, Is.EqualTo("TestParent"));
            Assert.That(result.Child.TestChildValue, Is.EqualTo("TestChild"));
        }

        [Test]
        public void TestTwoLevelsDeep()
        {
            var result = Build<IGrandParent>();
            Assert.That(result.TestGrandParentValue, Is.EqualTo("TestGrandParent"));
            Assert.That(result.Child.TestParentValue, Is.EqualTo("TestParent"));
            Assert.That(result.Child.Child.TestChildValue, Is.EqualTo("TestChild"));
        }

        public static T Build<T>() where T : class
        {
            var builder = ParamsBuilder<T>.New();
            var result = builder.Build();
            return result;
        }
    }
}