using System;
using NUnit.Framework;
using Paramulate.Attributes;
using Paramulate.ValueProviders;

namespace Paramulate.Test
{
    public interface IUserDatabaseParams
    {
        [Default("server=PROD;Database=UserDb")]
        string ConnectionString { get; }
        
        [Default("2")]
        int NumRetries { get; }
    }

    public enum TranslationMode
    {
        Fast = 1,
        Accurate = 2,
    }
    
    public interface ITranslatorParams
    {
        [Default("AutoDetect")]
        [Alias("i", "input-language", "(Optional) Use this to choose the input language. " +
                                            "Either 'AutoDetect' or a supported language (e.g. 'Spanish')")]
        string InputLanguage { get; }
        
        [Alias("o", "output-language", "Use this to choose the output language. " +
                                             "Any supported language (e.g. 'Mandarin')")]
        string OutputLanguage { get; }
        
        [Override("NumRetries", "3")]
        IUserDatabaseParams UserDb { get;}
        
        [Default("Fast")]
        [Alias("mode", "(Optional) Use this to choose the translation mode")]
        TranslationMode Mode { get;}
    }
    
    [TestFixture]
    public class TranslatorApp
    {
        public static int Mainy(string[] args)
        {
            var builder = ParamsBuilder<ITranslatorParams>.New("App",
                new CommandLineValueProvider(args)
            );
            var parameters = builder.Build();
            builder.WriteParams(parameters, Console.Out);
            // ... App Code ...
            return 0;
        }
        
        [Test]
        public void TestExample()
        {
            var args = new[] {"-o", "English", "--App.UserDb.ConnectionString", "Server=TEST;Database=UserDb7E2A"};
            Mainy(args);
        }
    }
}