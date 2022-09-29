// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Globalization;

namespace BiomStudio.ViewModels
{
    public sealed class ViewModelCollectionConverter : ExpandableObjectConverter
    {
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture,
            object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
