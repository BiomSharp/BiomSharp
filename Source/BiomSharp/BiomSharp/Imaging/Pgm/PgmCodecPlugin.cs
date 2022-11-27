// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Extensions;
using BiomSharp.Plugins;

namespace BiomSharp.Imaging.Pgm
{
    public sealed class PgmCodecPlugin<TFormat> : PgmCodec, IPlugin<TFormat>
        where TFormat : Enum
    {
        public string Name => "PGM";

        public TFormat? Id => Name.ToEnum<TFormat>();

        public string? Description =>
            "Portable grayscale map (P2/P5 supported; max - color = 255)";
    }
}
