// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BiomStudio.ViewModels
{
    public class FloatNumberRangeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string)
            {
                if ((context?.PropertyDescriptor
                    .Attributes.Cast<Attribute>()
                    .FirstOrDefault(attr => attr is RangeAttribute) ?? null)
                    is RangeAttribute rangeAttr)
                {
                    float floatValue = Convert.ToSingle(value);
                    return rangeAttr.IsValid(floatValue)
                        ?
                        floatValue
                        :
                        throw new FormatException(
                            context != null
                            ? rangeAttr.FormatErrorMessage(context.PropertyDescriptor.Name)
                            : rangeAttr.ErrorMessage);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context,
            CultureInfo? culture, object? value, Type destinationType)
            => destinationType == typeof(string)
            ? Convert.ToSingle(value).ToString()
            : base.ConvertTo(context, culture, value, destinationType);
    }
}
