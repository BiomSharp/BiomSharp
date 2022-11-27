// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Imaging.Wsq
{
    internal static class Math
    {
        internal static float FromBasePlusShift(uint value, int scale)
        {
            float s = value;
            while (scale > 0)
            {
                s /= 10;
                --scale;
            }
            return s;
        }

        internal static int ToBasePlusShiftInt(float value, out int scale, out byte sign)
        {
            byte s = 0;
            int v = 0;
            sign = 0;
            if (value < 0F)
            {
                sign = 1;
                value = -value;
            }
            if (value != 0F)
            {
                while (value < ushort.MaxValue)
                {
                    s += 1;
                    value *= 10F;
                }
                s -= 1;
                double dv = (double)value / 10D;
                v = (int)(dv < 0D ? dv - 0.5D : dv + 0.5D);
            }
            scale = s;
            return v;
        }

        internal static long ToBasePlusShiftLong(float value, out int scale, out byte sign)
        {
            byte s = 0;
            long v = 0L;
            sign = 0;
            if (value < 0F)
            {
                sign = 1;
                value = -value;
            }
            if (value != 0F)
            {
                while (value < uint.MaxValue)
                {
                    s += 1;
                    value *= 10F;
                }
                s -= 1;
                double dv = (double)value / 10D;
                v = (long)(dv < 0D ? dv - 0.5D : dv + 0.5D);
            }
            scale = s;
            return v;
        }

        internal static int SignOfPower(int power) => power == 0 ? 1 : power % 2 != 0 ? -1 : 1;
    }
}
