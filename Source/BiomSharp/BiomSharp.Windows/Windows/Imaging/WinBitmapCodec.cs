// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Drawing;
using System.Drawing.Imaging;

using BiomSharp.Extensions;
using BiomSharp.Imaging;
using BiomSharp.Windows.Imaging.Extensions;

namespace BiomSharp.Windows.Imaging
{
    public sealed class WinBitmapCodec<TFormat>
        :
        BitmapCodec<TFormat>, IDisposable
        where TFormat : Enum
    {
        private readonly ImageFormat imageFormat = default!;
        private Bitmap? bitmap;
        private bool disposedValue;

        public string Name { get; } = default!;

        public override string? Description { get; }

        public override string[]? FileExtensions { get; }

        public override TFormat? Id { get; }

        public WinBitmapCodec(string? format)
        {
            if (format != null)
            {
                ImageCodecInfo? imageCodecInfo =
                    ImageCodecInfo
                    .GetImageEncoders()
                    .FirstOrDefault(ci => ci.FormatDescription == format);
                if (imageCodecInfo != null)
                {
                    Name = imageCodecInfo.FormatDescription ?? "";
                    if (!string.IsNullOrEmpty(Name))
                    {
                        Id = Name.ToEnum<TFormat>();
                    }
                    imageFormat = new ImageFormat(imageCodecInfo.FormatID);
                    Description = imageCodecInfo.CodecName;
                    FileExtensions =
                        imageCodecInfo
                        .FilenameExtension
                        ?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ext => ext.ToLower()[1..])
                        .ToArray()
                        ??
                        Array.Empty<string>();
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                        bitmap = null;
                    }
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void FromRaw(SimpleBitmap? rawImage)
        {
            if (rawImage?.BitDepth is not 8 and not 24)
            {
                throw new InvalidOperationException(
                    "Raw image is null or pixel format unrecognized.");
            }
            Bitmap bitmap = rawImage.BitDepth switch
            {
                8 => new Bitmap(rawImage.Width, rawImage.Height,
                    PixelFormat.Format8bppIndexed),
                24 => new Bitmap(rawImage.Width, rawImage.Height,
                    PixelFormat.Format24bppRgb),
                _ => throw new NotImplementedException(),
            };
            if (rawImage.Resolution > 0)
            {
                bitmap.SetResolution(rawImage.Resolution, rawImage.Resolution);
            }
            using var wb = new WinBitmap(bitmap);
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                wb.SetGrayScalePalette();
            }
            wb.SetRawPixels(rawImage.Pixels);
            this.bitmap = wb.Release();
        }

        public override SimpleBitmap? ToRaw()
        {
            if (bitmap == null)
            {
                throw new InvalidOperationException("Bitmap is null");
            }
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                using var wb = new WinBitmap(bitmap);
                byte[]? pixels = wb.GetRawPixels();
                _ = wb.Release();
                return SimpleBitmap.Rgb24(
                    pixels, bitmap.Width, bitmap.Height,
                    (int)(bitmap.HorizontalResolution + 0.5F));
            }
            else if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                using var wb = new WinBitmap(bitmap);
                byte[]? pixels = wb.GetRawPixels();
                _ = wb.Release();
                return SimpleBitmap.Argb32(
                    pixels, bitmap.Width, bitmap.Height,
                    (int)(bitmap.HorizontalResolution + 0.5F));
            }
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                using var wb = new WinBitmap(bitmap);
                byte[]? pixels = wb.GetRawPixels();
                _ = wb.Release();
                var rawImage = SimpleBitmap.Gray8(pixels, bitmap.Width, bitmap.Height,
                    (int)(bitmap.HorizontalResolution + 0.5F));
                if (bitmap.Palette.Flags == 2)
                {
                    _ = rawImage.FromPal(bitmap.Palette);
                }
                return rawImage;
            }
            else
            {
                throw new InvalidOperationException(
                    "Bitmap pixel format invalid");
            }
        }

        public override byte[]? Encode()
        {
            if (bitmap != null)
            {
                using var stream = new MemoryStream();
                bitmap.Save(stream, imageFormat);
                return stream.ToArray();
            }
            return null;
        }

        public override byte[]? Encode<TParms>(TParms? writeParms) where TParms : class
            => Encode();

        public override void Decode(byte[] encoded)
        {
            using var stream = new MemoryStream(encoded);
            Read(stream);
        }

        public override void Decode<TParms>(byte[] encoded, out TParms? readParms)
            where TParms : class
        {
            readParms = null;
            Decode(encoded);
        }

        public override void Read(Stream stream)
        {
            using var bitmap = (Bitmap)Image.FromStream(stream);
            // Must clone, otherwise cannot Save() without getting exception:
            // "A generic error occurred in GDI+."
            this.bitmap = (Bitmap)bitmap.Clone();
        }

        public override void Read<TParms>(Stream stream, out TParms? readParms)
            where TParms : class
        {
            readParms = null;
            Read(stream);
        }

        public override void Write(Stream stream)
        {
            byte[]? encoded = Encode();
            if (encoded != null)
            {
                _ = stream.Seek(0, SeekOrigin.Begin);
                stream.Write(Encode());
            }
            else
            {
                throw new InvalidOperationException(
                    "Cannot write null image");
            }
        }

        public override void Write<TParms>(Stream stream, TParms? writeParms)
            where TParms : class
            =>
            Write(stream);
    }
}
