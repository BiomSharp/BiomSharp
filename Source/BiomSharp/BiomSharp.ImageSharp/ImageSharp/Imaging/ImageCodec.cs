// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Extensions;
using BiomSharp.ImageSharp.Imaging.Extensions;
using BiomSharp.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace BiomSharp.ImageSharp.Imaging
{
    public sealed class ImageCodec<TFormat>
        :
        BitmapCodec<TFormat>
        where TFormat : Enum
    {
        private readonly IImageFormat? format;
        private readonly IImageEncoder? encoder;
        //private readonly IImageDecoder? decoder;

        private Image? image = null;

        public ImageCodec(string format)
        {
            if (!string.IsNullOrEmpty(format) && format.TryToEnum(out TFormat? tFormat))
            {
                Id = tFormat;
                this.format = Configuration.Default.ImageFormatsManager
                    .ImageFormats.FirstOrDefault(fmt => fmt.Name == format);
                if (this.format != null)
                {
                    encoder = Configuration.Default.ImageFormatsManager.GetEncoder(this.format);
                    /*decoder*/
                    _ = Configuration.Default.ImageFormatsManager.GetDecoder(this.format);
                }
            }
        }

        public override TFormat? Id { get; }

        public override string? Description => "";

        public override string[]? FileExtensions
            => format?.FileExtensions.Select(e => e.StartsWith('.') ? e : $".{e}").ToArray();

        public override void Decode(byte[] encoded) => image = Image.Load(encoded);

        public override void Decode<TParms>(byte[] encoded, out TParms? readParms) where TParms : class
        {
            readParms = null;
            Decode(encoded);
        }

        public override byte[]? Encode()
        {
            if (encoder == null)
            {
                throw new InvalidOperationException("Encoder is null");
            }
            using var stream = new MemoryStream();
            image?.Save(stream, encoder);
            return stream.ToArray();
        }

        public override byte[]? Encode<TParms>(TParms? _) where TParms : class
            => Encode();

        public override void FromRaw(SimpleBitmap? raw)
        {
            if (raw != null)
            {
                if (raw.IsColor)
                {
                    image = raw.ToImageRgb24();
                }
                else if (raw.IsGray)
                {
                    image = raw.ToImageL8();
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(raw)} is null or invalid");
            }
        }

        public override void Read(Stream stream)
        {
            ImageInfo imageInfo = Image.Identify(stream);
            _ = stream.Seek(0, SeekOrigin.Begin);
            image = imageInfo.PixelType.BitsPerPixel switch
            {
                8 => Image.Load<L8>(stream),
                24 => Image.Load<Rgb24>(stream),
                32 => Image.Load<Argb32>(stream),
                _ => throw new InvalidOperationException(
                        $"{nameof(stream)} is not an 8, 24 or 32 bit image"),
            };
        }

        public override void Read<TParms>(Stream stream, out TParms? readParms) where TParms : class
        {
            readParms = null;
            Read(stream);
        }

        public override SimpleBitmap? ToRaw()
        {
            if (image != null)
            {
                if (image is Image<Argb32> argb32)
                {
                    return argb32.ToRaw();
                }
                else if (image is Image<Rgb24> rgb24)
                {
                    return rgb24.ToRaw();
                }
                else if (image is Image<L8> l8)
                {
                    return l8.ToRaw();
                }
            }
            throw new InvalidOperationException(
                $"Failed to create raw image");
        }

        public override void Write(Stream stream) =>
            image?.Save(stream, encoder ?? throw new InvalidOperationException("Encoder is null"));

        public override void Write<TParms>(Stream stream, TParms? _) where TParms : class
            => Write(stream);
    }
}
