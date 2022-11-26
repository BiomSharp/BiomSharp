// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Drawing.Imaging;

namespace BiomStudio.Controls
{
    public class HandprintRenderer
    {
        #region Appearance settings

        public Color BackColor { get; set; }
        public Color RidgeEndColor { get; set; }
        public Color BifurcationColor { get; set; }
        public Color UnknownEndColor { get; set; }
        public int MinutiaOutlineOpacity { get; set; }
        public int HandprintOpacity { get; set; }

        #endregion Appearance settings

        public Bitmap? DisplayImage { get; private set; }
        public ScrollablePicturePanel PicturePanel { get; private set; }
        public bool DisplayMinutiaeFlag { get; }
        public bool DisplayImageFlag { get; }
        public Bitmap? HandprintImage { get; }

        public HandprintRenderer(
            ScrollablePicturePanel picturePanel,
            Bitmap? handprintImage,
            int fingerprintOpacity,
            bool displayImageFlag = true,
            bool displayMinutiaeFlag = true)
        {
            PicturePanel = picturePanel;
            HandprintImage = handprintImage;
            DisplayImageFlag = displayImageFlag;
            DisplayMinutiaeFlag = displayMinutiaeFlag;
            RidgeEndColor = Color.Red;
            BifurcationColor = Color.Green;
            UnknownEndColor = Color.Yellow;
            MinutiaOutlineOpacity = 100;
            BackColor = Color.White;
            HandprintOpacity = fingerprintOpacity;
        }

        public HandprintRenderer RenderImageToDisplay()
        {
            if (HandprintImage != null)
            {
                using Bitmap handPrintImage =
                    HandprintImage.Clone(
                        new Rectangle(Point.Empty, HandprintImage.Size),
                        PixelFormat.Format24bppRgb);

                Bitmap targetImage = new(
                    handPrintImage.Width, handPrintImage.Height,
                    PixelFormat.Format24bppRgb);
                using var graphics = Graphics.FromImage(targetImage);
                graphics.Clear(BackColor);
                ColorMatrix matrix = new()
                {
                    Matrix33 = HandprintOpacity / 100f
                };
                ImageAttributes attributes = new();
                attributes.SetColorMatrix(matrix,
                    ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(handPrintImage,
                    new Rectangle(Point.Empty, targetImage.Size),
                    0, 0, targetImage.Width, targetImage.Height,
                    GraphicsUnit.Pixel, attributes);

                if (DisplayImage != null)
                {
                    PicturePanel.Image = null;
                    DisplayImage.Dispose();
                }
                DisplayImage = targetImage;
                PicturePanel.Image = DisplayImage;
            }
            return this;
        }
    }
}
