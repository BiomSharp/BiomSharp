// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Runtime.CompilerServices;
using BiomSharp.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BiomSharp.ImageSharp.Imaging.Extensions
{
    public static class ImageExtensions
    {
        public static SimpleBitmap ToRaw(this Image<Argb32> bitmap)
        {
            byte[] pixels = new byte[bitmap.Width * bitmap.Height * 4];
            bitmap.ProcessPixelRows(pa =>
            {
                int outIndex = 0;
                for (int y = 0; y < pa.Height; y++)
                {
                    Span<Argb32> pixelRow = pa.GetRowSpan(y);
                    foreach (ref Argb32 pixel in pixelRow)
                    {
                        pixels[outIndex++] = pixel.B;
                        pixels[outIndex++] = pixel.G;
                        pixels[outIndex++] = pixel.R;
                        pixels[outIndex++] = pixel.A;
                    }
                }
            });
            return SimpleBitmap.Argb32(pixels, bitmap.Width, bitmap.Height);
        }

        public static SimpleBitmap ToRaw(this Image<Rgb24> bitmap)
        {
            byte[] pixels = new byte[bitmap.Width * bitmap.Height * 3];
            bitmap.ProcessPixelRows(pa =>
            {
                int outIndex = 0;
                for (int y = 0; y < pa.Height; y++)
                {
                    Span<Rgb24> pixelRow = pa.GetRowSpan(y);
                    foreach (ref Rgb24 pixel in pixelRow)
                    {
                        pixels[outIndex++] = pixel.B;
                        pixels[outIndex++] = pixel.G;
                        pixels[outIndex++] = pixel.R;
                    }
                }
            });
            return SimpleBitmap.Rgb24(pixels, bitmap.Width, bitmap.Height);
        }

        public static SimpleBitmap ToRaw(this Image<L8> bitmap)
        {
            byte[] pixels = new byte[bitmap.Width * bitmap.Height * Unsafe.SizeOf<L8>()];
            bitmap.CopyPixelDataTo(pixels);
            return SimpleBitmap.Gray8(pixels, bitmap.Width, bitmap.Height);
        }

        public static Image<Rgb24> ToImageRgb24(this SimpleBitmap rawImage)
            => Image.LoadPixelData<Rgb24>(
                (byte[])rawImage.Pixels.Clone(), rawImage.Width, rawImage.Height);

        public static Image<L8> ToImageL8(this SimpleBitmap rawImage)
            => Image.LoadPixelData<L8>(
                (byte[])rawImage.Pixels.Clone(), rawImage.Width, rawImage.Height);
    }
}
