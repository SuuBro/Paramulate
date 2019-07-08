using System;
using NUnit.Framework;
using Paramulate.ValueProviders;

namespace Paramulate.Test
{
    [TestFixture]
    public class TestHelp
    {
        [Test]
        public void TestHelpText()
        {
            var args = new[] {"-h"};

            void CheckHelpText(string s)
            {
                Console.WriteLine(s);
                Assert.That(s, Is.EqualTo(
@"-i  --input-language:  (Optional) Use this to choose the input language. Either 'AutoDetect' or a supported language (e.g. 'Spanish')
-o  --output-language:  Use this to choose the output language. Any supported language (e.g. 'Mandarin')
     --mode:  (Optional) Use this to choose the translation mode
"));
                Assert.Pass();
            }

            ParamsBuilder<ITranslatorParams>.New("App",
                CheckHelpText, 
                new CommandLineValueProvider(args)
            );
            Assert.Fail(); // Expect Assert.Pass from callback
        }
    }
}