// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Drawing.Imaging;
using BiomSharp.Imaging;
using BiomSharp.Plugins;

namespace BiomSharp.Windows.Imaging
{
    public sealed class WinBitmapCodecFactory<TFormat, TBitmapCodec>
        :
        BitmapCodecFactory<TFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        public WinBitmapCodecFactory()
            => ImageCodecInfo
            .GetImageEncoders().Select(ie => ie.FormatDescription)
            .Intersect(ImageCodecInfo.GetImageDecoders()
                .Select(id => id.FormatDescription))
            .ToList()
            .ForEach(format =>
            {
                if (format != null)
                {
                    var codec = new WinBitmapCodec<TFormat>(format);
                    if (codec.Id != null && codec.Id.ToString() == codec.Name)
                    {
                        this[codec.Id] = codec;
                    }
                }
            });
    }
}
