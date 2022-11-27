// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Extensions
{
    public static class StringExtensions
    {
        public static TEnum? ToEnum<TEnum>(this string? name) where TEnum : Enum
            => name.TryToEnum(out TEnum? tEnum) ? tEnum : default;

        public static bool TryToEnum<TEnum>(this string? name, out TEnum? tEnum)
        {
            tEnum = default;
            string? enumName = Enum
                .GetNames(typeof(TEnum))
                .Select(e => e.ToUpper())
                .FirstOrDefault(e => e == name?.ToUpper());
            if (enumName != null
                &&
                Enum.TryParse(typeof(TEnum), name, out object? tEnum1)
                &&
                tEnum1 != null)
            {
                tEnum = (TEnum?)tEnum1;
            }
            return tEnum != null;
        }
    }
}
