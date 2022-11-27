// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Extensions
{
    public static class UnitConverter
    {
        public const float CmPerInch = 2.54f;

        public static int ToPerCm(this int value) => (int)((value / CmPerInch) + 0.5f);
    }
}
