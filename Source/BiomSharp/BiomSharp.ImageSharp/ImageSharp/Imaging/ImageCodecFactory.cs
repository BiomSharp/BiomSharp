// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging;
using BiomSharp.Plugins;
using SixLabors.ImageSharp;

namespace BiomSharp.ImageSharp.Imaging
{
    public sealed class ImageCodecFactory<TFormat, TBitmapCodec>
        :
        BitmapCodecFactory<TFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        public ImageCodecFactory()
            => Configuration.Default.ImageFormatsManager
            .ImageFormats.ToList()
            .ForEach(format =>
            {
                var codec = new ImageCodec<TFormat>(format.Name);
                if (codec.Id != null && codec.Id.ToString() == format.Name)
                {
                    this[codec.Id] = codec;
                }
            });
    }
}
