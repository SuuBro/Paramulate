using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Paramulate.Attributes;

namespace Paramulate.Test
{
    [Paramulate]
    public interface ISimple
    {
        [Default("110011")]
        int AnIntFromDefault { get; set; }

        [Default("Here Is A Value Mate")]
        string AString { get; set; }
    }

    [Paramulate]
    public interface IPrintParent
    {
        [Default("120021")]
        int ParentLevelInt { get; set; }

        [Override("AString", "I came from IPrintParent")]
        ISimple Child1 { get; set; }

        [Default("I'm on the parent")]
        string ParentLevelString { get; set; }
    }

    [TestFixture]
    public class TestPrinting
    {
        [Test]
        public void TestPrintingSimple()
        {
            var builder = ParamsBuilder<ISimple>.New("RootName");
            var testObject = builder.Build();
            var testStream = new TextMessageWriter();
            builder.WriteParams(testObject, testStream);

            Console.WriteLine(testStream.ToString());

            Assert.That(testStream.ToString(), Is.EqualTo(
@"RootName:
  AnIntFromDefault: 110011 (From Default)
  AString: ""Here Is A Value Mate"" (From Default)
"
            ));
        }

        [Test]
        public void TestPrintingNested()
        {
            var builder = ParamsBuilder<IPrintParent>.New("PrintParent");
            var testObject = builder.Build();
            var testStream = new TextMessageWriter();
            builder.WriteParams(testObject, testStream);

            Console.WriteLine(testStream.ToString());

            Assert.That(testStream.ToString(), Is.EqualTo(
@"PrintParent:
  ParentLevelInt: 120021 (From Default)
  Child1:
    AnIntFromDefault: 110011 (From Default)
    AString: ""I came from IPrintParent"" (From Override in IPrintParent)

  ParentLevelString: ""I'm on the parent"" (From Default)
"
            ));
        }

    }
}