using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Paramulate.Attributes;

namespace Paramulate.Test
{
    public interface IDeep
    {
        string Deeper { get; set; }
    }

    public enum When
    {
        Now,
        Later,
    }
    
    public interface ISimple
    {
        [Default("Now")]
        When When { get; set; }
        
        [Default("110011")]
        int AnIntFromDefault { get; set; }

        [Default("Here Is A Value Mate")]
        string AString { get; set; }
        
        [Override("Deeper", "I should be ignored")]
        IDeep Deep { get; }
        
        [Override("Deeper", "I came from Derp")]
        IDeep Derp { get; }
    }

    public interface IPrintParent
    {
        [Default("120021")]
        int ParentLevelInt { get; set; }

        [Override("AString", "I came from IPrintParent")]
        [Override("Deep.Deeper", "I came from IPrintParent too")]
        ISimple Child1 { get; set; }

        [Default("I'm on the parent")]
        string ParentLevelString { get; set; }
    }

    [TestFixture]
    public class TestPrinting
    {
        [Test]
        public void TestPrintingObject()
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
    When: ""Now"" (From Default)
    AnIntFromDefault: 110011 (From Default)
    AString: ""I came from IPrintParent"" (From Override on Child1)
    Deep:
      Deeper: ""I came from IPrintParent too"" (From Override on Child1)

    Derp:
      Deeper: ""I came from Derp"" (From Override on Child1.Derp)

  ParentLevelString: ""I'm on the parent"" (From Default)
"
            ));
        }

    }
}