// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Drawing.Imaging;

using BiomSharp.Biometrics;
using BiomSharp.Biometrics.Hand;

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
        public IBiometricTemplate<IHandMinutia>? HandTemplate { get; set; }

        public HandprintRenderer(
            ScrollablePicturePanel picturePanel,
            Bitmap? handprintImage,
            IBiometricTemplate<IHandMinutia>? handTemplate,
            int fingerprintOpacity,
            bool displayImageFlag = true,
            bool displayMinutiaeFlag = true)
        {
            PicturePanel = picturePanel;
            HandprintImage = handprintImage;
            HandTemplate = handTemplate;
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
            using Bitmap handPrintImage =
                HandprintImage?.Clone(
                    new Rectangle(Point.Empty, HandprintImage.Size),
                    PixelFormat.Format24bppRgb)
                ??
                new Bitmap(
                    HandTemplate?.SourceBounds.Width ?? 0,
                    HandTemplate?.SourceBounds.Height ?? 0,
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
            if (DisplayMinutiaeFlag && HandTemplate != null)
            {
                DrawMinutiae(graphics, handPrintImage, HandTemplate.Features);
            }
            //pictureBox.BackColor = BackColor;
            if (DisplayImage != null)
            {
                PicturePanel.Image = null;
                DisplayImage.Dispose();
            }
            DisplayImage = targetImage;
            PicturePanel.Image = DisplayImage;
            return this;
        }

        private void DrawMinutiae(
            Graphics graphics,
            Bitmap handPrintImage,
            IList<IHandMinutia> minutiae)
        {
            int minutiaOpacity =
                (int)((255.0f * MinutiaOutlineOpacity / 100.0f) + 0.5f);
            using Pen ridgeEndPen =
                new(Color.FromArgb(minutiaOpacity, RidgeEndColor), 1.0F);
            using Pen bifurcationPen =
                new(Color.FromArgb(minutiaOpacity, BifurcationColor), 1.0F);
            using Pen unknownEndPen =
                new(Color.FromArgb(minutiaOpacity, UnknownEndColor), 1.0F);
            foreach (IHandMinutia minutia in minutiae)
            {
                int y = handPrintImage.Height - minutia.Y;
                int xs = minutia.X;
                int ys = y;
                double angle = 180f - minutia.Theta;
                angle *= Math.PI / 180f;
                int xd = Convert.ToInt32(10f * Math.Cos(angle));
                int yd = Convert.ToInt32(10f * Math.Sin(angle));
                int xe = minutia.X + xd;
                int ye = y + yd;
                Pen pen = unknownEndPen;
                switch (minutia.Type)
                {
                    case HandMinutiaType.Bifurcation:
                        pen = bifurcationPen;
                        break;
                    case HandMinutiaType.RidgeEnd:
                        pen = ridgeEndPen;
                        break;
                    case HandMinutiaType.Unknown:
                        break;
                    default:
                        break;
                }
                graphics.DrawRectangle(pen, minutia.X - 4, y - 4, 8, 8);
                graphics.DrawLine(pen, xs, ys, xe, ye);
            }
        }
    }
}
