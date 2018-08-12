<h1 align="center">Paramulate</h1>

<div align="center">
  <strong>A framework for parameterising C# components and applications</strong>
</div>

<br />

<div align="center">
  <a href="https://travis-ci.org/SuuBro/Paramulate">
    <img src="https://travis-ci.com/SuuBro/Paramulate.svg?branch=master"
      alt="Build Status" />
  </a>
  <a href="https://www.nuget.org/packages/Paramulate">
    <img src="https://img.shields.io/nuget/v/Paramulate.svg"
      alt="Nuget" />
  </a>
</div>

<div align="center">
  <sub>Written by 
  <a href="https://www.linkedin.com/in/jsubramaniam/">Joshua Subramaniam</a> and
  <a href="https://github.com/SuuBro/Paramulate/graphs/contributors">
    contributors
  </a>
</div>

## Table of Contents
- [Summary](#summary)
- [Philosophy](#philosophy)
- [Example](#example)
- [Features](#features)

## Summary
Paramulate is a powerful framework for parameterising C# code, from command line arguments, programmatic defaults, config files, databases or any other source you like! It allows you to easily print your parameters to an output stream and see where individual settings came from.

## Philosophy
Paramulate creates a culture around the design of your components with respect to parameterisation. It hopes to inspire developers to follow these mantras when writing applications and libraries:
- __Separate logic:__ Paramulate encourages developers to separate business logic from configuration and settings, behaviour should be agnostic to the input mechanism
- __Author knows best:__ sensible defaults should be provided by developers to make using their component easier
- __Customer knows better:__ advanced users what to change everything, and they know their use-case best
- __Seeing is believing:__ you should be able to easily see the inputs to the code
- __I'm no Sherlock:__ tracking down the source of parameters should be trivial
- __Consistency without constraint:__ Paramulate encourages consistency, but doesn't force a dependency

## Example
Given the code:
```csharp
    public interface IUserDatabaseParams
    {
        [Default("server=PROD;Database=UserDb")]
        string ConnectionString { get; }
        
        [Default("2")]
        int NumRetries { get; }
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
    }
    
    public class TranslatorApp
    {
        public static int Main(string[] args)
        {
            var builder = ParamsBuilder<ITranslatorParams>.New("App",
                new CommandLineValueProvider(args)
            );
            var parameters = builder.Build();
            builder.WriteParams(parameters, Console.Out);
            // ... App Code ...
            return 0;
        }
    }
```
and the command
```bash
TranslatorApp.exe -o English --App.UserDb.ConnectionString "Server=TEST;Database=UserDb7E2A"
```
You would have the following parameter object initilaised for you:
```text
App:
  InputLanguage: "AutoDetect" (From Default)
  OutputLanguage: "English" (From Command Line)
  UserDb:
    ConnectionString: "Server=TEST;Database=UserDb7E2A" (From Command Line)
    NumRetries: 3 (From Override on UserDb)
```

## Features
TODO
