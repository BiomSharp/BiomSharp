// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Extensions;
using BiomSharp.Plugins;

namespace BiomSharp.Imaging.Wsq
{
    public sealed class WsqCodecPlugin<TFormat> : WsqCodec, IPlugin<TFormat>
        where TFormat : Enum
    {
        public WsqCodecPlugin() { }

        public string Name => "WSQ";

        public string? Description => "NIST/FBI Wavelet Scalar Quantization format";

        public TFormat? Id => Name.ToEnum<TFormat>();
    }
}
