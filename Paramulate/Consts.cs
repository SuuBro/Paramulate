﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Paramulate.Test")]

namespace Paramulate
{
    internal class Consts
    {
        internal const char PathSeperator = '.';
        internal const string RootNameField = "|ROOTNAME|";
        internal const string SourceMetadata = "|SOURCE";
    }
}