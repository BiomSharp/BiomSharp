// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Imaging.Wsq.Tree
{
    public class Filter
    {
        public static readonly Filter Odd7x9 = new FilterOdd7x9();
        public static readonly Filter Even8x8 = new FilterEven8x8() { Name = "8x8" };
        public string Name { get; protected set; } = "7x9";
        public float[] Hi { get; protected set; } = FilterOdd7x9.DefaultHi;
        public float[] Lo { get; protected set; } = FilterOdd7x9.DefaultLo;

        protected Filter() { }

        public static Filter Create(string name) => name.ToLower() switch
        {
            "7x9" => Odd7x9,
            "8x8" => Even8x8,
            _ => throw new WsqCodecException(
                    "Invalid filter name: use '7x9' or '8x8'"),
        };

        public static Filter Create(float[] lo, float[] hi)
        {
            var filter = new Filter()
            {
                Hi = (float[])hi.Clone(),
                Lo = (float[])lo.Clone(),
                Name = string.Format("{0}x{1}", lo.Length, hi.Length),
            };
            return filter;
        }
    }

    internal class FilterOdd7x9 : Filter
    {
        public static readonly float[] DefaultHi = new float[]
        {
            0.06453888262893845F,
            -0.04068941760955844F,
            -0.41809227322221221F,
            0.78848561640566439F,
            -0.41809227322221221F,
            -0.04068941760955844F,
            0.06453888262893845F
        };

        public static readonly float[] DefaultLo = new float[]
        {
            0.03782845550699546F,
            -0.02384946501938000F,
            -0.11062440441842342F,
            0.37740285561265380F,
            0.85269867900940344F,
            0.37740285561265380F,
            -0.11062440441842342F,
            -0.02384946501938000F,
            0.03782845550699546F
        };

        public FilterOdd7x9()
        {
            Hi = DefaultHi;
            Lo = DefaultLo;
        }
    }

    internal class FilterEven8x8 : Filter
    {
        public static readonly float[] DefaultHi = new float[]
        {
            0.03226944131446922F,
            -0.05261415011924844F,
            -0.18870142780632693F,
            0.60328894481393847F,
            -0.60328894481393847F,
            0.18870142780632693F,
            0.05261415011924844F,
            -0.03226944131446922F
        };

        public static readonly float[] DefaultLo = new float[]
        {
            0.07565691101399093F,
            -0.12335584105275092F,
            -0.09789296778409587F,
            0.85269867900940344F,
            0.85269867900940344F,
            -0.09789296778409587F,
            -0.12335584105275092F,
            0.07565691101399093F
        };

        public FilterEven8x8()
        {
            Hi = DefaultHi;
            Lo = DefaultLo;
        }
    }
}
