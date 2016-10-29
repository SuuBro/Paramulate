using NUnit.Framework;
using Paramulate.Attributes;
using Paramulate.Exceptions;

namespace Paramulate.Test
{
    [Paramulate]
    public interface ILevel1Params
    {
        string Level1 { get; }

        [Override("Level2", "OverrideFromLevel1")]
        [Override("Level3Params.Level3", "OverrideFromLevel1")]
        ILevel2Params Level2Params { get; }
    }

    [Paramulate]
    public interface ILevel2Params
    {
        [Default("OverrideMe")]
        string Level2 { get; }

        [Override("Level3Int", "11")]
        [Override("Level3", "OverrideFromLevel2")]
        ILevel3Params Level3Params { get; }
    }

    [Paramulate]
    public interface ILevel3Params
    {
        string Level3 { get; }

        [Default("0")]
        int Level3Int { get; }
    }

    public interface IInvalidOverrideParams
    {
        [Override("Level2Params.Level3Params.LevelDoesntExist", "IRRELEVANT")]
        ILevel1Params Params { get; }
    }

    public interface IInvalidOverrideValueParams
    {
        [Override("Level2Params.Level3Params.Level3Int", "NotAnInt")]
        ILevel1Params Params { get; }
    }


    [TestFixture]
    public class TestOverrides
    {
        [Test]
        public void TestSingleLevelOverride()
        {
            var level2 = TestUtils.Build<ILevel2Params>();
            Assert.That(level2.Level3Params.Level3, Is.EqualTo("OverrideFromLevel2"));
            Assert.That(level2.Level3Params.Level3Int, Is.EqualTo(11));
        }

        [Test]
        public void TestTwoLevelOverrideReplacesOneLevelOverride()
        {
            var level1 = TestUtils.Build<ILevel1Params>();
            Assert.That(level1.Level2Params.Level3Params.Level3, Is.EqualTo("OverrideFromLevel1"));
            Assert.That(level1.Level2Params.Level3Params.Level3Int, Is.EqualTo(11));
        }

        [Test]
        public void TestOverrideReplacesDefault()
        {
            var level1 = TestUtils.Build<ILevel1Params>();
            Assert.That(level1.Level2Params.Level2, Is.EqualTo("OverrideFromLevel1"));
        }

        [Test]
        public void TestInvalidOverridePath()
        {
            Assert.That(TestUtils.Build<IInvalidOverrideParams>,
                Throws.TypeOf<InvalidPropertySpecifierException>()
                      .With.Message.EqualTo("Could not find the property 'LevelDoesntExist' the type" +
                                            " 'Paramulate.Test.ILevel3Params'. Please chack the specifier " +
                                            "'Level2Params.Level3Params.LevelDoesntExist' on property 'Params'" +
                                            " in the type 'Paramulate.Test.IInvalidOverrideParams'"));
        }

        [Test]
        public void TestInvalidOverrideValue()
        {
            Assert.That(TestUtils.Build<IInvalidOverrideValueParams>,
                Throws.TypeOf<InvalidProvidedValueException>()
                    .With.Message.EqualTo("Failed to deserialize value 'NotAnInt' for property" +
                                          " 'T.Params.Level2Params.Level3Params.Level3Int' (type:System.Int32)" +
                                          " when setting override from property 'Params' in type" +
                                          " 'Paramulate.Test.IInvalidOverrideValueParams'"));
        }
    }
}