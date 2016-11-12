using System;
using System.IO;
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
        string AStringFromDefault { get; set; }
    }

    [TestFixture]
    public class TestPrinting
    {
        [Test]
        public void TestPrintingSimple()
        {
            var builder = ParamsBuilder<ISimple>.New();
            var testObject = builder.Build("RootName");
            var testStream = new TextMessageWriter();
            builder.WriteParams(testObject, testStream);

            Assert.That(testStream.ToString(), Is.EqualTo(
@"RootName
  AnIntFromDefault: 110011 (From Default)
  AStringFromDefault: ""Here Is A Value Mate"" (From Default)
"
            ));

            Console.WriteLine(testStream.ToString());
        }

    }
}