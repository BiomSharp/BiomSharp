// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

namespace BiomStudio.Controls
{
    public class ScrollablePicturePanel : Panel
    {
        public enum PicturePanelSizeMode
        {
            Fit, Actual,
        };

        public event EventHandler? ImageChanged;

        private readonly IContainer components = default!;

        private PicturePanelSizeMode sizeMode;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private readonly PictureBox pictureBox = default!;

        public Image? Image
        {
            get => pictureBox.Image;
            set
            {
                pictureBox.Image = value;
                UpdateSizeMode();
                ImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateSizeMode()
        {
            pictureBox.Location = Point.Empty;
            if (Image != null)
            {
                if (Image.Width > Size.Width
                    ||
                    Image.Height > Size.Height)
                {
                    if (sizeMode == PicturePanelSizeMode.Fit)
                    {
                        pictureBox.Dock = DockStyle.Fill;
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pictureBox.Dock = DockStyle.None;
                        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        if (Image.Width < Size.Width)
                        {
                            pictureBox.Left = (Size.Width - Image.Width) / 2;
                        }
                        if (Image.Height < Size.Height)
                        {
                            pictureBox.Top = (Size.Height - Image.Height) / 2;
                        }
                    }
                }
                else
                {
                    if (sizeMode == PicturePanelSizeMode.Fit)
                    {
                        pictureBox.Dock = DockStyle.Fill;
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    else
                    {
                        pictureBox.Dock = DockStyle.Fill;
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                }
            }
        }

        public PicturePanelSizeMode SizeMode
        {
            get => sizeMode;
            set
            {
                sizeMode = value;
                UpdateSizeMode();
            }
        }

        /*
        public PicturePanelSizeMode SizeMode
        {
            get => pictureBox.SizeMode == PictureBoxSizeMode.Zoom
                    ?
                    PicturePanelSizeMode.Fit : PicturePanelSizeMode.Actual;
            set
            {
                if (value == PicturePanelSizeMode.Fit)
                {
                    pictureBox.Dock = DockStyle.Fill;
                    pictureBox.SizeMode =
                        Image != null
                        ? Image.Width > pictureBox.ClientSize.Width
                          ||
                          Image.Height > pictureBox.ClientSize.Height
                        ? PictureBoxSizeMode.Zoom
                        : PictureBoxSizeMode.CenterImage
                        : PictureBoxSizeMode.Zoom;
                }
                else
                {
                    if (Image != null)
                    {
                        if (Image.Width >= pictureBox.ClientSize.Width
                            ||
                            Image.Height >= pictureBox.ClientSize.Height)
                        {
                            pictureBox.Dock = DockStyle.None;
                            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        }
                        else
                        {
                            pictureBox.Dock = DockStyle.Fill;
                            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                        }
                    }
                }
            }
        }
        */

        public ScrollablePicturePanel()
        {
            pictureBox = new PictureBox();
            ((ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();

            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "PictureBox";
            pictureBox.Size = new Size(300, 200);
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;

            AutoScroll = true;
            BackColor = Color.Transparent;
            Controls.Add(pictureBox);
            Name = "ScrollablePictureBox";
            Size = new Size(300, 200);
            ((ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            UpdateSizeMode();
        }
    }
}
