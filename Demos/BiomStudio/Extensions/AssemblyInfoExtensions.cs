// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Globalization;
using System.Reflection;

[AttributeUsage(AttributeTargets.Assembly)]
internal class BuildDateAttribute : Attribute
{
    public BuildDateAttribute(string value) => DateTime = DateTime.ParseExact(
            value,
            "yyyyMMddHHmmss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);

    public DateTime DateTime { get; }
}

[AttributeUsage(AttributeTargets.Assembly)]
internal class AssemblyNamedVersionAttribute : Attribute
{
    public string Value { get; private set; }

    public AssemblyNamedVersionAttribute() : this("") { }
    public AssemblyNamedVersionAttribute(string value) => Value = value;
}

namespace BiomStudio.Extensions
{
    internal static class AssemblyInfoExtensions
    {
        public static string? AssemblyTitle(this Assembly asm) => asm.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

        public static string? AssemblyCopyright(this Assembly asm) => asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        public static string? AssemblyVersion(this Assembly asm) => asm.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;

        public static string? AssemblyFileVersion(this Assembly asm) => asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

        public static string? AssemblyNamedVersion(this Assembly asm) => asm.GetCustomAttribute<AssemblyNamedVersionAttribute>()?.Value;

        public static DateTime AssemblyBuildDate(this Assembly asm) => asm.GetCustomAttribute<BuildDateAttribute>()?.DateTime ?? default;
    }
}
