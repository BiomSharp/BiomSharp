// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;

using BiomSharp.Imaging;

namespace BiomStudio.ViewModels
{
    [ReadOnly(true)]
    [DisplayName("Bitmap Image")]
    [Description("Bitmap image information")]
    public class SimpleBitmapViewModel
    {
        [DisplayName("Width")]
        [Description("Number of pixels in horizontal direction")]
        [DefaultValue(0)]
        [ReadOnly(true)]
        public int Width { get; } = 0;

        [DisplayName("Height")]
        [Description("Number of pixels in vertical direction")]
        [DefaultValue(0)]
        [ReadOnly(true)]
        public int Height { get; } = 0;

        [DisplayName("DPI")]
        [Description("Image resolution (pixels-per-inch)")]
        [DefaultValue(-1)]
        [ReadOnly(true)]
        public int Resolution { get; } = -1;

        [DisplayName("Pixel Bit Depth")]
        [Description("Number of bits-per-pixel (packed)")]
        [ReadOnly(true)]
        public int BitDepth { get; } = 0;

        [DisplayName("Is Gray")]
        [Description("Classified as gray - bit-depth = 8")]
        [ReadOnly(true)]
        public bool IsGray => BitDepth == 8;

        [DisplayName("Is Color")]
        [Description("Classified as color - bit-depth = 24")]
        [ReadOnly(true)]
        public bool IsColor => BitDepth == 24;

        public SimpleBitmapViewModel(SimpleBitmap bitmap)
        {
            BitDepth = bitmap.BitDepth;
            Height = bitmap.Height;
            Width = bitmap.Width;
            Resolution = bitmap.Resolution;
        }
    }
}
