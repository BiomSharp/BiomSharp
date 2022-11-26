// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging;
using BiomSharp.Nist;
using BiomSharp.Nist.Nfiq;
using BiomSharp.Plugins;
using BiomSharp.Windows.Imaging.Extensions;

using BiomStudio.Controls;
using BiomStudio.Data;
using BiomStudio.Factories.Imaging;
using BiomStudio.Properties;
using BiomStudio.ViewModels;

namespace BiomStudio.Forms
{
    public partial class MainForm : Form
    {
        private PluginCodecFactory<BitmapCodec<BitmapFormat>>? bitmapFactory;

        private SimpleBitmap? rawImage;

        public MainForm()
        {
            InitializeComponent();

            Text =
                $"{ApplicationData.Title} - {ApplicationData.NamedVersion}";
            CopyrightLabel.Text = ApplicationData.Copyright;

            FxSelectorComboBox.SelectedIndex =
                Enum.TryParse(Settings.Default.ImagingFx, out ImagingFx imagingFx)
                ? (int)imagingFx : 0;

        }

        private void ShowImageContrastTooltip(bool show)
        {
            if (show)
            {
                Point mousePos = ImageOpacityTrackBar.Control.PointToClient(Cursor.Position);
                ImageOpacityTrackBarToolTip.Show(
                    $"Image opacity: {ImageOpacityTrackBar.Value}%",
                    ImageOpacityTrackBar.Control,
                    mousePos.X,
                    mousePos.Y + 36);
            }
            else
            {
                ImageOpacityTrackBarToolTip.Hide(ImageOpacityTrackBar.Control);
            }
        }

        private Bitmap CreateDisplayImage(SimpleBitmap rawImage)
        {
            if (rawImage.IsColor && ImageAsPrint.Checked)
            {
                rawImage = SimpleBitmap.ToGray(rawImage)!;
            }
            return rawImage.ToBitmap();
        }

        private void RenderImageToDisplay(Bitmap bitmap)
            =>
            _ = new HandprintRenderer(ImagePanel, bitmap, ImageOpacityTrackBar.Value)
            .RenderImageToDisplay();

        private void LoadImage(string filePath)
        {
            IBitmapCodec? codec = bitmapFactory?.CreateFromFilePath(filePath);
            if (codec != null)
            {
                var plugin = (IPlugin<BitmapFormat>)codec;
                using FileStream stream = File.OpenRead(filePath);
                codec.Read(stream, out object? readParms);
                rawImage = codec.ToRaw();
                if (rawImage != null)
                {
                    Bitmap? bitmap = CreateDisplayImage(rawImage);
                    ReadImagePropertyGrid.SelectedObject = null;
                    if (bitmap != null)
                    {
                        ImageOpacityTrackBar.Value = 100;
                        RenderImageToDisplay(bitmap);
                        var gridObjs = new ViewModelCollection(filePath)
                        {
                            new SimpleBitmapViewModel(rawImage!),
                        };
                        if (readParms != null)
                        {
                            switch (plugin.Id)
                            {
                                case BitmapFormat.PGM:
                                    gridObjs.Add(new PgmParametersViewModel(readParms, true));
                                    break;
                                case BitmapFormat.WSQ:
                                    gridObjs.Add(new WsqParametersViewModel(readParms, true));
                                    break;
                                case BitmapFormat.BMP:
                                    break;
                                case BitmapFormat.PBM:
                                    break;
                                case BitmapFormat.PNG:
                                    break;
                                case BitmapFormat.JPEG:
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (ImageAsPrint.Checked && Nfiq2.Initialized)
                        {
                            Nfiq2.GetVersion(out string libNfiq2Version, out string _);
                            if (Nfiq2.ComputeAll(
                                out Nfiq2Analysis? nfiq2Analysis, out int _,
                                (int)NistFingerCode.AnyFinger, rawImage!) == 0)
                            {
                                gridObjs.Add(new Nfiq2ParametersViewModel(
                                    libNfiq2Version, nfiq2Analysis!));
                            }
                        }
                        ReadImagePropertyGrid.SelectedObject = gridObjs;
                        ReadImagePropertyGrid.ExpandAllGridItems();
                    }
                }
            }
        }

        private void SaveImage(string filePath)
        {

        }

        private void OnFileExit(object sender, EventArgs e) => Close();

        private void OnImageFileOpen(object sender, EventArgs e)
        {
            ImageFileOpenFileDialog.Filter = bitmapFactory?.FileDialogFilter;
            ImageFileOpenFileDialog.InitialDirectory = Settings.Default.SourceFolder;
            if (string.IsNullOrEmpty(ImageFileOpenFileDialog.InitialDirectory))
            {
                ImageFileOpenFileDialog.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            ImageFileOpenFileDialog.FilterIndex = 0;
            if (ImageFileOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageFileOpenFileDialog.InitialDirectory =
                    Path.GetDirectoryName(ImageFileOpenFileDialog.FileName);
                Settings.Default.SourceFolder = ImageFileOpenFileDialog.InitialDirectory;
                Settings.Default.Save();

                LoadImage(ImageFileOpenFileDialog.FileName);
            }
        }

        private void OnImageSizeModeFit(object sender, EventArgs e)
            => ImagePanel.SizeMode = ScrollablePicturePanel.PicturePanelSizeMode.Fit;

        private void OnImageSizeModeFitUpdateUI(object sender, EventArgs e)
        {
            ((FormCommand)sender).DefaultUpdateUI(ImagePanel.Image != null);
            ((FormCommand)sender).Checked = ImagePanel.SizeMode == ScrollablePicturePanel.PicturePanelSizeMode.Fit;
        }

        private void OnImageSizeModeActual(object sender, EventArgs e)
            => ImagePanel.SizeMode = ScrollablePicturePanel.PicturePanelSizeMode.Actual;

        private void OnImageSizeModeActualUpdateUI(object sender, EventArgs e)
        {
            ((FormCommand)sender).DefaultUpdateUI(ImagePanel.Image != null);
            ((FormCommand)sender).Checked = ImagePanel.SizeMode == ScrollablePicturePanel.PicturePanelSizeMode.Actual;
        }

        private void FxSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FxSelectorComboBox.SelectedIndex >= 0)
            {
                var imagingFx = (ImagingFx)FxSelectorComboBox.SelectedIndex;
                bitmapFactory = new(imagingFx);
                Settings.Default.ImagingFx = imagingFx.ToString();
                Settings.Default.Save();
            }
        }

        private void ImageAsPrint_Command(object sender, EventArgs e)
        {
            ((FormCommand)sender).Checked = !((FormCommand)sender).Checked;
            if (!string.IsNullOrEmpty(ImageFileOpenFileDialog.FileName))
            {
                LoadImage(ImageFileOpenFileDialog.FileName);
            }
        }

        private void ImageAsPrint_UpdateUI(object sender, EventArgs e)
            => ((FormCommand)sender).DefaultUpdateUI(true);

        private void ImageOpacityTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (rawImage != null)
            {
                ShowImageContrastTooltip(true);
                RenderImageToDisplay(CreateDisplayImage(rawImage));
            }
        }

        private void ImageOpacityTrackBar_MouseHover(object sender, EventArgs e)
            => ShowImageContrastTooltip(true);

        private void ImageOpacityTrackBar_MouseLeave(object sender, EventArgs e)
            => ImageOpacityTrackBarToolTip.Hide(ImageOpacityTrackBar.Control);

        private void ImageOpacityTrackBar_MouseUp(object sender, MouseEventArgs e)
            => ShowImageContrastTooltip(false);

        private void ImagePanel_ImageChanged(object sender, EventArgs e)
            => ImageOpacityTrackBar.Enabled = ImagePanel.Image != null;
    }
}
