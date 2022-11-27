// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Drawing;
using System.Drawing.Imaging;

using BiomSharp.Imaging;

namespace BiomSharp.Windows.Imaging.Extensions
{
    public static class BitmapExtensions
    {
        public static SimpleBitmap ToRaw(this Bitmap bitmap)
        {
            byte[]? pixels;
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                using var wb = new WinBitmap(bitmap);
                pixels = wb.GetRawPixels();
                _ = wb.Release();
                return SimpleBitmap.Rgb24(
                    pixels ?? Array.Empty<byte>(),
                    bitmap.Width, bitmap.Height,
                    (int)(bitmap.HorizontalResolution + 0.5F));
            }
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                using var wb = new WinBitmap(bitmap);
                pixels = wb.GetRawPixels();
                _ = wb.Release();
                // TODO: Assumes all 8bpp images are gray.
                return SimpleBitmap.Gray8(
                    pixels ?? Array.Empty<byte>(),
                    bitmap.Width, bitmap.Height,
                    (int)(bitmap.HorizontalResolution + 0.5F));
            }
            else
            {
                throw new InvalidOperationException(
                    "Bitmap pixel format invalid");
            }
        }

        public static Bitmap ToBitmap(this SimpleBitmap rawImage)
        {
            int depth = rawImage.BitDepth;
            if (depth is not 8 and not 24 and not 32)
            {
                throw new InvalidOperationException(
                    "Image pixel format unrecognized.");
            }
            Bitmap bitmap = depth switch
            {
                8 => new Bitmap(rawImage.Width, rawImage.Height,
                    PixelFormat.Format8bppIndexed),
                24 => new Bitmap(rawImage.Width, rawImage.Height,
                    PixelFormat.Format24bppRgb),
                32 => new Bitmap(rawImage.Width, rawImage.Height,
                    PixelFormat.Format32bppArgb),
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
            return wb.Release()!;
        }
    }
}
