// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Globalization;

using BiomSharp.Nist;

namespace BiomStudio.ViewModels
{
    public class FingerprintResolutionConverter : TypeConverter
    {
        private readonly List<int> resolutions = new() { NistConstants.NcmPpiUnknown, 500, 1000 };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) => true;

        public override StandardValuesCollection? GetStandardValues(ITypeDescriptorContext? context)
            => new(resolutions);

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object? ConvertFrom(ITypeDescriptorContext? context,
            CultureInfo? culture, object value)
            => value is string strVal
            ? strVal == "Unknown" ? NistConstants.NcmPpiUnknown : Convert.ToInt32(strVal)
            : base.ConvertFrom(context, culture, value);

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) => true;

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            => destinationType == typeof(int) || base.CanConvertTo(context, destinationType);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture,
            object? value, Type destinationType)
            => destinationType == typeof(string) && value is int intVal
            ? intVal == NistConstants.NcmPpiUnknown ? "Unknown" : intVal.ToString()
            : base.ConvertTo(context, culture, value, destinationType);
    }
}
