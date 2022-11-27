// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Nist;

namespace BiomSharp.Imaging
{
    public class SimpleBitmap
    {
        public static SimpleBitmap Gray8(byte[]? pixels, int width, int height,
            int resolution = NistConstants.NcmPpiUnknown)
            => new()
            {
                Pixels = pixels ?? new byte[width * height],
                Width = width,
                Height = height,
                Resolution = resolution,
                BitDepth = 8,
            };

        public static SimpleBitmap Rgb24(byte[]? pixels, int width, int height,
            int resolution = NistConstants.NcmPpiUnknown)
            => new()
            {
                Pixels = pixels ?? new byte[width * height * 3],
                Width = width,
                Height = height,
                Resolution = resolution,
                BitDepth = 24,
            };

        public static SimpleBitmap Argb32(byte[]? pixels, int width, int height,
            int resolution = NistConstants.NcmPpiUnknown)
            => new()
            {
                Pixels = pixels ?? new byte[width * height * 4],
                Width = width,
                Height = height,
                Resolution = resolution,
                BitDepth = 32,
            };

        private static byte[]? RgbToBgr(byte[]? rgbPixels)
        {
            if (rgbPixels == null)
            {
                return null;
            }
            byte[] bgrPixels = (byte[])rgbPixels.Clone();
            for (int i = 0; i < bgrPixels.Length / 3; i += 3)
            {
                (bgrPixels[i + 2], bgrPixels[i]) = (bgrPixels[i], bgrPixels[i + 2]);
            }
            return bgrPixels;
        }

        private static byte[]? ArgbToBgra(byte[]? argbPixels)
        {
            if (argbPixels == null)
            {
                return null;
            }
            byte[] bgraPixels = (byte[])argbPixels.Clone();
            for (int i = 0; i < bgraPixels.Length / 4; i += 4)
            {
                (bgraPixels[i + 3], bgraPixels[i]) = (bgraPixels[i], bgraPixels[i + 3]);
                (bgraPixels[i + 2], bgraPixels[i + 1]) = (bgraPixels[i + 1], bgraPixels[i + 2]);
            }
            return bgraPixels;
        }

        public static SimpleBitmap Bgr24(byte[]? pixels, int width, int height,
            int resolution = NistConstants.NcmPpiUnknown)
            => new()
            {
                Pixels = RgbToBgr(pixels) ?? new byte[width * height * 3],
                Width = width,
                Height = height,
                Resolution = resolution,
                BitDepth = 24,
            };

        public static SimpleBitmap Bgra32(byte[]? pixels, int width, int height,
            int resolution = NistConstants.NcmPpiUnknown)
            => new()
            {
                Pixels = ArgbToBgra(pixels) ?? new byte[width * height * 4],
                Width = width,
                Height = height,
                Resolution = resolution,
                BitDepth = 24,
            };

        public byte[] Pixels { get; protected set; } = default!;

        public int Width { get; protected set; } = 0;

        public int Height { get; protected set; } = 0;

        public int Resolution { get; protected set; } = NistConstants.NcmPpiUnknown;

        public int BitDepth { get; protected set; } = 0;

        public bool IsGray => BitDepth is 8;

        public bool IsColor => BitDepth is 24 or 32;

        public SimpleBitmap Clone()
            => IsGray || IsColor
            ? new SimpleBitmap()
            {
                BitDepth = BitDepth,
                Width = Width,
                Height = Height,
                Resolution = Resolution,
                Pixels = (byte[])Pixels.Clone(),
            }
            : throw new InvalidOperationException("Image bit-depth is not 8, 24 or 32");

        public static SimpleBitmap? ToGray(SimpleBitmap? raw)
        {
            if (raw != null)
            {
                if (raw.IsColor)
                {
                    SimpleBitmap gray = Gray8(null, raw.Width, raw.Height, raw.Resolution);

                    if (raw.BitDepth == 24)
                    {
                        for (int o = 0; o < gray.Pixels.Length; o++)
                        {
                            int i = (o << 1) + o;
                            gray.Pixels[o] = (byte)(((299 * raw.Pixels[i]) +
                                (587 * raw.Pixels[i + 1]) + (114 * raw.Pixels[i + 2])) / 1000);
                        }
                    }
                    else
                    {
                        for (int o = 0; o < gray.Pixels.Length; o++)
                        {
                            int i = o << 2;
                            gray.Pixels[o] = (byte)(((299 * raw.Pixels[i + 1]) +
                                (587 * raw.Pixels[i + 2]) + (114 * raw.Pixels[i + 3])) / 1000);
                        }
                    }
                    return gray;
                }
                else if (raw.IsGray)
                {
                    return raw;
                }
                else
                {
                    throw new InvalidOperationException("Image bit-depth is not 8, 24 or 32");
                }
            }
            return default;
        }
    }
}
