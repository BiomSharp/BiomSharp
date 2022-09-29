// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Drawing.Imaging;

using BiomSharp.Imaging;

namespace BiomSharp.Windows.Imaging.Extensions
{
    internal static class SimpleBitmapExtensions
    {
        public static SimpleBitmap FromPal(this SimpleBitmap gray, ColorPalette palette)
        {
            int[] palMap;
            if (palette.Entries.Length < 256)
            {
                int[] hist = new int[256];
                for (int i = 0; i < gray.Pixels.Length; i++)
                {
                    ++hist[gray.Pixels[i]];
                }
                palMap = new int[hist.Count(h => h != 0)];
                int palIdx = 0;
                for (int i = 0; i < hist.Length; i++)
                {
                    if (hist[i] > 0)
                    {
                        palMap[palIdx++] = i;
                    }
                }
            }
            else
            {
                palMap = new int[256];
                for (int i = 0; i < 256; i++)
                {
                    palMap[i] = i;
                }
            }
            for (int i = 0; i < gray.Pixels.Length; i++)
            {
                gray.Pixels[i] = palette.Entries[palMap[gray.Pixels[i]]].R;
            }
            return gray;
        }
    }
}
