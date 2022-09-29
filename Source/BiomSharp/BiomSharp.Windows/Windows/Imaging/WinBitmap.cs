// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BiomSharp.Windows.Imaging
{
    public class WinBitmap : IDisposable
    {
        private Bitmap? bitmap;
        private BitmapData? bitmapData;
        private byte[]? pixels;

        public int BytesPerPixel { get; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        // TODO: Assume AR 1:1
        public int Resolution =>
            bitmap != null ? (int)bitmap.HorizontalResolution : 0;

        public WinBitmap(Bitmap? bitmap)
        {
            if (bitmap != null)
            {
                if (bitmap.PixelFormat
                    is PixelFormat.Format32bppArgb
                    or PixelFormat.Format24bppRgb
                    or PixelFormat.Format8bppIndexed)
                {
                    this.bitmap = bitmap;
                    BytesPerPixel = Image.GetPixelFormatSize(this.bitmap.PixelFormat) / 8;
                    Width = this.bitmap.Width;
                    Height = this.bitmap.Height;
                }
                else
                {
                    throw new InvalidOperationException("Bitmap not 8, 24 or 32 bpp");
                }
            }
            else
            {
                throw new InvalidOperationException("Bitmap is null");
            }
        }

        public WinBitmap(int width, int height, PixelFormat pixelFormat)
        {
            if (pixelFormat is PixelFormat.Format24bppRgb or
                PixelFormat.Format8bppIndexed)
            {
                bitmap = new Bitmap(width, height, pixelFormat);
                BytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                Width = width;
                Height = height;
            }
            else
            {
                throw new InvalidOperationException("Bitmap not 8 or 24 bpp");
            }
        }

        public bool Locked { get; private set; }

        public void Lock()
        {
            if (!Locked && bitmap != null)
            {
                Locked = true;
                bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadWrite, bitmap.PixelFormat);
                Height = bitmapData.Height;
                Width = bitmapData.Width * BytesPerPixel;
                pixels = new byte[bitmapData.Stride * bitmap.Height];
                Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);
            }
        }

        public void UnLock()
        {
            if (Locked
                &&
                bitmap != null
                &&
                bitmapData != null
                &&
                pixels != null)
            {
                Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
                bitmap.UnlockBits(bitmapData);
                pixels = null;
                bitmapData = null;
                Locked = false;
            }
        }

        public Bitmap? Release()
        {
            UnLock();
            Bitmap? bitmap = this.bitmap;
            this.bitmap = null;
            return bitmap;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPalPixel(int x, int y, byte palColor)
        {
            if (pixels != null && bitmapData != null)
            {
                pixels[(y * bitmapData.Stride) + (x * BytesPerPixel)] = palColor;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetPalPixel(int x, int y)
        {
            if (pixels != null && bitmapData != null)
            {
                return pixels[(y * bitmapData.Stride) + (x * BytesPerPixel)];
            }
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRgbPixel(int x, int y, byte b, byte g, byte r)
        {
            if (pixels != null && bitmapData != null)
            {
                int index = (y * bitmapData.Stride) + (x * BytesPerPixel);
                pixels[index] = b;
                pixels[index + 1] = g;
                pixels[index + 2] = r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRgbPixel(int x, int y, Color color)
        {
            if (pixels != null && bitmapData != null)
            {
                int index = (y * bitmapData.Stride) + (x * BytesPerPixel);
                pixels[index] = color.B;
                pixels[index + 1] = color.G;
                pixels[index + 2] = color.R;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetRgbPixel(int x, int y)
        {
            if (pixels != null && bitmapData != null)
            {
                int index = (y * bitmapData.Stride) + (x * BytesPerPixel);
                return Color.FromArgb(
                    pixels[index + 2],
                    pixels[index + 1],
                    pixels[index]);
            }
            return default;
        }

        public void SetPalette(ColorPalette palette)
        {
            if (bitmap?.PixelFormat == PixelFormat.Format8bppIndexed
                &&
                palette.Entries.Length == 256)
            {
                ColorPalette pal = bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    pal.Entries[i] = palette.Entries[i];
                }
                bitmap.Palette = pal;
            }
        }

        public void SetGrayScalePalette()
        {
            if (bitmap?.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette pal = bitmap.Palette;
                for (int i = 0; i < pal.Entries.Length; i++)
                {
                    pal.Entries[i] = Color.FromArgb(255, i, i, i);
                }
                bitmap.Palette = pal;
            }
        }

        public void ConvertToGrayScale()
        {
            if (bitmap == null)
            {
                throw new InvalidOperationException("Bitmap is null");
            }
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                Bitmap grey = new(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
                grey.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                using var g = Graphics.FromImage(grey);
                var greyMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] {.299f, .299f, .299f, 0, 0},
                        new float[] {.587f, .587f, .587f, 0, 0},
                        new float[] {.114f, .114f, .114f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });
                using (var attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(greyMatrix);
                    g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                       0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);
                }
                g.Dispose();
                grey.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                bitmap.Dispose();
                bitmap = grey;
            }
            else
            {
                throw new InvalidOperationException("Bit-depth must be 24");
            }
        }

        public void ConvertToRgb()
        {
            if (bitmap == null)
            {
                throw new InvalidOperationException("Bitmap is null");
            }
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                Bitmap rgb = bitmap.Clone(
                    new Rectangle(Point.Empty, bitmap.Size),
                    PixelFormat.Format24bppRgb);
                rgb.SetResolution(
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution);
                bitmap.Dispose();
                bitmap = rgb;
            }
        }

        public byte[]? GetRawPixels()
        {
            if (bitmap != null)
            {
                byte[]? rawPix = null;
                Lock();
                if (bitmapData != null)
                {
                    rawPix = new byte[Height * Width];
                    for (int i = 0, s = 0, d = 0;
                            i < bitmap.Height;
                            i++, s += bitmapData.Stride, d += Width)
                    {
                        pixels.AsSpan(s, Width).CopyTo(rawPix.AsSpan(d, Width));
                    }
                }
                UnLock();
                return rawPix;
            }
            return null;
        }

        public byte[,]? GetRawImage()
        {
            byte[]? rawPixels = GetRawPixels();
            if (rawPixels != null)
            {
                byte[,] rawImage = new byte[Height, Width];
                Buffer.BlockCopy(rawPixels, 0, rawImage, 0, rawPixels.Length);
                return rawImage;
            }
            return null;
        }

        public void SetRawImage(byte[,] rawImage)
        {
            if (rawImage.GetLength(0) != Height ||
                rawImage.GetLength(1) != Width)
            {
                throw new InvalidOperationException(
                    "BitmapImage and raw image dimensions differ.");
            }
            byte[] rawPixels = new byte[Height * Width];
            Buffer.BlockCopy(rawImage, 0, rawPixels, 0, rawImage.Length);
            SetRawPixels(rawPixels);
        }

        public void SetRawPixels(byte[] rawPix)
        {
            if (bitmap != null)
            {
                Lock();
                if (bitmapData != null)
                {
                    for (int i = 0, s = 0, d = 0;
                            i < bitmap.Height;
                            i++, s += Width, d += bitmapData.Stride)
                    {
                        pixels.AsSpan(d, bitmapData.Stride).Clear();
                        rawPix
                            .AsSpan(s, Width)
                            .CopyTo(pixels.AsSpan(d, Width));
                    }
                }
                UnLock();
            }
        }

        public WinBitmap Clone()
        {
            if (bitmap != null)
            {
                Lock();
                var bmi = new WinBitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
                if (bmi.bitmap != null)
                {
                    bmi.bitmap.SetResolution(
                        bitmap.HorizontalResolution,
                        bitmap.VerticalResolution);
                    if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                    {
                        bmi.bitmap.Palette = bitmap.Palette;
                    }
                    if (pixels != null && bmi.pixels != null)
                    {
                        bmi.Lock();
                        pixels.CopyTo(bmi.pixels, 0);
                        bmi.UnLock();
                    }
                }
                UnLock();
                return bmi;
            }
            throw new InvalidOperationException("Bitmap is null");
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (bitmap != null)
                    {
                        if (Locked)
                        {
                            UnLock();
                        }

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

        #endregion
    }
}
