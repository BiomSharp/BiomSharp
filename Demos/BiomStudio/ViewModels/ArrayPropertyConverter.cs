// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;
using System.Globalization;

namespace BiomStudio.ViewModels
{
    public sealed class ArrayPropertyConverter : ArrayConverter
    {
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture,
            object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                return value is Array array ? $"{array.Length} {context?.PropertyDescriptor.Name}" : "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection? GetProperties(
            ITypeDescriptorContext? context, object? value, Attribute[]? attributes)
        {
            if (value is Array array && context?.Instance is IViewModel viewModel)
            {
                return new PropertyDescriptorCollection(
                    base.GetProperties(context, value, attributes)
                    .Cast<PropertyDescriptor>()
                    .Select(pd => new ViewModelPropertyDescriptor(pd, viewModel))
                    .ToArray());
            }
            else
            {
                return base.GetProperties(context, value, attributes);
            }
        }
    }
}
