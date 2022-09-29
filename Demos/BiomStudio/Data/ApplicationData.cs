// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Reflection;
using BiomStudio.Extensions;

namespace BiomStudio.Data
{
    // The supported bitmap formats
    internal enum BitmapFormat
    {
        BMP, PBM, PGM, PNG, JPEG, WSQ
    };

    internal static class ApplicationData
    {
        public static readonly string? Title
            = Assembly.GetEntryAssembly()?.AssemblyTitle();
        public static readonly string? NamedVersion
            = Assembly.GetEntryAssembly()?.AssemblyNamedVersion();
        public static readonly string? Copyright
            = Assembly.GetEntryAssembly()?.AssemblyCopyright();
        public static readonly DateTime BuildDate
            = Assembly.GetEntryAssembly()?.AssemblyBuildDate() ?? default;
    }
}
