using BiomStudio.Controls;

namespace BiomStudio.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.CopyrightLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenuMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.FileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditPasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewImageSizeModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageSizeModeFitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageSizeModeActualMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsOptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainButtonStrip = new System.Windows.Forms.ToolStrip();
            this.FileOpenButton = new System.Windows.Forms.ToolStripButton();
            this.FileSaveButton = new System.Windows.Forms.ToolStripButton();
            this.MainButtonSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.EditCopyButton = new System.Windows.Forms.ToolStripButton();
            this.EditPasteButton = new System.Windows.Forms.ToolStripButton();
            this.MainButtonSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewFitButton = new System.Windows.Forms.ToolStripButton();
            this.ViewActualButton = new System.Windows.Forms.ToolStripButton();
            this.MainButtonSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolsOptionsButton = new System.Windows.Forms.ToolStripButton();
            this.FxSelectorComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.FxSelectorLabel = new System.Windows.Forms.ToolStripLabel();
            this.MainButtonSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ImageOpacityTrackBar = new BiomStudio.Controls.ToolStripTrackBarItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ImageAsFingerprintButton = new System.Windows.Forms.ToolStripButton();
            this.MainContentPanel = new System.Windows.Forms.Panel();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.ReadImageTabPage = new System.Windows.Forms.TabPage();
            this.ReadImagePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.WriteImageTabPage = new System.Windows.Forms.TabPage();
            this.WriteImagePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ImagePanel = new BiomStudio.Controls.ScrollablePicturePanel();
            this.FileExit = new BiomStudio.Controls.FormCommand();
            this.ImageFileOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ImageFileOpen = new BiomStudio.Controls.FormCommand();
            this.ImageSizeModeFit = new BiomStudio.Controls.FormCommand();
            this.ImageSizeModeActual = new BiomStudio.Controls.FormCommand();
            this.ImageAsPrint = new BiomStudio.Controls.FormCommand();
            this.ImageOpacityTrackBarToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MainStatusStrip.SuspendLayout();
            this.MainMenuMenuStrip.SuspendLayout();
            this.MainButtonStrip.SuspendLayout();
            this.MainContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.ReadImageTabPage.SuspendLayout();
            this.WriteImageTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageFileOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSizeModeFit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSizeModeActual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageAsPrint)).BeginInit();
            this.SuspendLayout();
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.BackColor = System.Drawing.Color.Transparent;
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyrightLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 539);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(884, 22);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 0;
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainMenuMenuStrip
            // 
            this.MainMenuMenuStrip.BackColor = System.Drawing.Color.White;
            this.MainMenuMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.EditMenuItem,
            this.ViewMenuItem,
            this.MainToolsMenuItem,
            this.MainHelpMenuItem});
            this.MainMenuMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuMenuStrip.Name = "MainMenuMenuStrip";
            this.MainMenuMenuStrip.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.MainMenuMenuStrip.Size = new System.Drawing.Size(884, 25);
            this.MainMenuMenuStrip.TabIndex = 1;
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileOpenMenuItem,
            this.MainMenuSeparator0,
            this.FileSaveMenuItem,
            this.FileSaveAsMenuItem,
            this.MainMenuSeparator1,
            this.FileExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 19);
            this.FileMenuItem.Text = "&File";
            // 
            // FileOpenMenuItem
            // 
            this.FileOpenMenuItem.Image = global::BiomStudio.Properties.Resources.ic_open_24dp;
            this.FileOpenMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FileOpenMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.FileOpenMenuItem.Name = "FileOpenMenuItem";
            this.FileOpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.FileOpenMenuItem.Size = new System.Drawing.Size(154, 30);
            this.FileOpenMenuItem.Text = "&Open";
            this.FileOpenMenuItem.ToolTipText = "Open File";
            // 
            // MainMenuSeparator0
            // 
            this.MainMenuSeparator0.Name = "MainMenuSeparator0";
            this.MainMenuSeparator0.Size = new System.Drawing.Size(151, 6);
            // 
            // FileSaveMenuItem
            // 
            this.FileSaveMenuItem.Image = global::BiomStudio.Properties.Resources.ic_save_24dp;
            this.FileSaveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FileSaveMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.FileSaveMenuItem.Name = "FileSaveMenuItem";
            this.FileSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.FileSaveMenuItem.Size = new System.Drawing.Size(154, 30);
            this.FileSaveMenuItem.Text = "&Save";
            this.FileSaveMenuItem.ToolTipText = "Save File";
            // 
            // FileSaveAsMenuItem
            // 
            this.FileSaveAsMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FileSaveAsMenuItem.Name = "FileSaveAsMenuItem";
            this.FileSaveAsMenuItem.Size = new System.Drawing.Size(154, 30);
            this.FileSaveAsMenuItem.Text = "Save &As";
            this.FileSaveAsMenuItem.ToolTipText = "Save File As";
            // 
            // MainMenuSeparator1
            // 
            this.MainMenuSeparator1.Name = "MainMenuSeparator1";
            this.MainMenuSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FileExitMenuItem.Name = "FileExitMenuItem";
            this.FileExitMenuItem.Size = new System.Drawing.Size(154, 30);
            this.FileExitMenuItem.Text = "E&xit";
            this.FileExitMenuItem.ToolTipText = "Exit Application";
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditCopyMenuItem,
            this.EditPasteMenuItem});
            this.EditMenuItem.Name = "EditMenuItem";
            this.EditMenuItem.Size = new System.Drawing.Size(39, 19);
            this.EditMenuItem.Text = "&Edit";
            // 
            // EditCopyMenuItem
            // 
            this.EditCopyMenuItem.Image = global::BiomStudio.Properties.Resources.ic_copy_24dp;
            this.EditCopyMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EditCopyMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.EditCopyMenuItem.Name = "EditCopyMenuItem";
            this.EditCopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.EditCopyMenuItem.Size = new System.Drawing.Size(152, 30);
            this.EditCopyMenuItem.Text = "&Copy";
            this.EditCopyMenuItem.ToolTipText = "Copy";
            // 
            // EditPasteMenuItem
            // 
            this.EditPasteMenuItem.Image = global::BiomStudio.Properties.Resources.ic_paste_24dp;
            this.EditPasteMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EditPasteMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.EditPasteMenuItem.Name = "EditPasteMenuItem";
            this.EditPasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.EditPasteMenuItem.Size = new System.Drawing.Size(152, 30);
            this.EditPasteMenuItem.Text = "&Paste";
            this.EditPasteMenuItem.ToolTipText = "Paste";
            // 
            // ViewMenuItem
            // 
            this.ViewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewImageSizeModeMenuItem});
            this.ViewMenuItem.Name = "ViewMenuItem";
            this.ViewMenuItem.Size = new System.Drawing.Size(44, 19);
            this.ViewMenuItem.Text = "&View";
            // 
            // ViewImageSizeModeMenuItem
            // 
            this.ViewImageSizeModeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImageSizeModeFitMenuItem,
            this.ImageSizeModeActualMenuItem});
            this.ViewImageSizeModeMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ViewImageSizeModeMenuItem.Name = "ViewImageSizeModeMenuItem";
            this.ViewImageSizeModeMenuItem.Size = new System.Drawing.Size(161, 22);
            this.ViewImageSizeModeMenuItem.Text = "Image SizeMode";
            // 
            // ImageSizeModeFitMenuItem
            // 
            this.ImageSizeModeFitMenuItem.Image = global::BiomStudio.Properties.Resources.ic_fit_24dp;
            this.ImageSizeModeFitMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ImageSizeModeFitMenuItem.Name = "ImageSizeModeFitMenuItem";
            this.ImageSizeModeFitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
            this.ImageSizeModeFitMenuItem.Size = new System.Drawing.Size(195, 30);
            this.ImageSizeModeFitMenuItem.Text = "Fit In Window";
            this.ImageSizeModeFitMenuItem.ToolTipText = "Fit In Window";
            // 
            // ImageSizeModeActualMenuItem
            // 
            this.ImageSizeModeActualMenuItem.Image = global::BiomStudio.Properties.Resources.ic_size_actual_24dp;
            this.ImageSizeModeActualMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ImageSizeModeActualMenuItem.Name = "ImageSizeModeActualMenuItem";
            this.ImageSizeModeActualMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
            this.ImageSizeModeActualMenuItem.Size = new System.Drawing.Size(195, 30);
            this.ImageSizeModeActualMenuItem.Text = "Actual Size";
            this.ImageSizeModeActualMenuItem.ToolTipText = "Actual Size";
            // 
            // MainToolsMenuItem
            // 
            this.MainToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsOptionsMenuItem});
            this.MainToolsMenuItem.Name = "MainToolsMenuItem";
            this.MainToolsMenuItem.Size = new System.Drawing.Size(46, 19);
            this.MainToolsMenuItem.Text = "&Tools";
            // 
            // ToolsOptionsMenuItem
            // 
            this.ToolsOptionsMenuItem.Image = global::BiomStudio.Properties.Resources.ic_settings_24dp;
            this.ToolsOptionsMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolsOptionsMenuItem.Name = "ToolsOptionsMenuItem";
            this.ToolsOptionsMenuItem.Size = new System.Drawing.Size(124, 30);
            this.ToolsOptionsMenuItem.Text = "&Options";
            this.ToolsOptionsMenuItem.ToolTipText = "Options";
            // 
            // MainHelpMenuItem
            // 
            this.MainHelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpAboutMenuItem});
            this.MainHelpMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MainHelpMenuItem.Name = "MainHelpMenuItem";
            this.MainHelpMenuItem.Size = new System.Drawing.Size(44, 19);
            this.MainHelpMenuItem.Text = "&Help";
            // 
            // HelpAboutMenuItem
            // 
            this.HelpAboutMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.HelpAboutMenuItem.Name = "HelpAboutMenuItem";
            this.HelpAboutMenuItem.Size = new System.Drawing.Size(116, 22);
            this.HelpAboutMenuItem.Text = "&About...";
            // 
            // MainButtonStrip
            // 
            this.MainButtonStrip.AllowMerge = false;
            this.MainButtonStrip.BackColor = System.Drawing.Color.White;
            this.MainButtonStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainButtonStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainButtonStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileOpenButton,
            this.FileSaveButton,
            this.MainButtonSeparator0,
            this.EditCopyButton,
            this.EditPasteButton,
            this.MainButtonSeparator1,
            this.ViewFitButton,
            this.ViewActualButton,
            this.MainButtonSeparator2,
            this.ToolsOptionsButton,
            this.FxSelectorComboBox,
            this.FxSelectorLabel,
            this.MainButtonSeparator6,
            this.ImageOpacityTrackBar,
            this.toolStripSeparator1,
            this.ImageAsFingerprintButton});
            this.MainButtonStrip.Location = new System.Drawing.Point(0, 25);
            this.MainButtonStrip.Name = "MainButtonStrip";
            this.MainButtonStrip.Size = new System.Drawing.Size(884, 31);
            this.MainButtonStrip.Stretch = true;
            this.MainButtonStrip.TabIndex = 2;
            // 
            // FileOpenButton
            // 
            this.FileOpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FileOpenButton.Image = global::BiomStudio.Properties.Resources.ic_open_24dp;
            this.FileOpenButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.FileOpenButton.Name = "FileOpenButton";
            this.FileOpenButton.Size = new System.Drawing.Size(28, 28);
            this.FileOpenButton.Text = "&Open";
            this.FileOpenButton.ToolTipText = "Open File";
            // 
            // FileSaveButton
            // 
            this.FileSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FileSaveButton.Image = global::BiomStudio.Properties.Resources.ic_save_24dp;
            this.FileSaveButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.FileSaveButton.Name = "FileSaveButton";
            this.FileSaveButton.Size = new System.Drawing.Size(28, 28);
            this.FileSaveButton.Text = "&Save";
            this.FileSaveButton.ToolTipText = "Save File";
            // 
            // MainButtonSeparator0
            // 
            this.MainButtonSeparator0.Name = "MainButtonSeparator0";
            this.MainButtonSeparator0.Size = new System.Drawing.Size(6, 31);
            // 
            // EditCopyButton
            // 
            this.EditCopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditCopyButton.Image = global::BiomStudio.Properties.Resources.ic_copy_24dp;
            this.EditCopyButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.EditCopyButton.Name = "EditCopyButton";
            this.EditCopyButton.Size = new System.Drawing.Size(28, 28);
            this.EditCopyButton.Text = "&Copy";
            // 
            // EditPasteButton
            // 
            this.EditPasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditPasteButton.Image = global::BiomStudio.Properties.Resources.ic_paste_24dp;
            this.EditPasteButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.EditPasteButton.Name = "EditPasteButton";
            this.EditPasteButton.Size = new System.Drawing.Size(28, 28);
            this.EditPasteButton.Text = "&Paste";
            // 
            // MainButtonSeparator1
            // 
            this.MainButtonSeparator1.Name = "MainButtonSeparator1";
            this.MainButtonSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // ViewFitButton
            // 
            this.ViewFitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ViewFitButton.Image = global::BiomStudio.Properties.Resources.ic_fit_24dp;
            this.ViewFitButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ViewFitButton.Name = "ViewFitButton";
            this.ViewFitButton.Size = new System.Drawing.Size(28, 28);
            this.ViewFitButton.Text = "Fit In Window";
            this.ViewFitButton.ToolTipText = "Fit In Window";
            // 
            // ViewActualButton
            // 
            this.ViewActualButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ViewActualButton.Image = global::BiomStudio.Properties.Resources.ic_size_actual_24dp;
            this.ViewActualButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ViewActualButton.Name = "ViewActualButton";
            this.ViewActualButton.Size = new System.Drawing.Size(28, 28);
            this.ViewActualButton.Text = "Actual Size";
            this.ViewActualButton.ToolTipText = "Actual Size";
            // 
            // MainButtonSeparator2
            // 
            this.MainButtonSeparator2.Name = "MainButtonSeparator2";
            this.MainButtonSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // ToolsOptionsButton
            // 
            this.ToolsOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolsOptionsButton.Image = global::BiomStudio.Properties.Resources.ic_settings_24dp;
            this.ToolsOptionsButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolsOptionsButton.Name = "ToolsOptionsButton";
            this.ToolsOptionsButton.Size = new System.Drawing.Size(28, 28);
            this.ToolsOptionsButton.Text = "Options";
            // 
            // FxSelectorComboBox
            // 
            this.FxSelectorComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.FxSelectorComboBox.AutoSize = false;
            this.FxSelectorComboBox.BackColor = System.Drawing.Color.White;
            this.FxSelectorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FxSelectorComboBox.Items.AddRange(new object[] {
            "BioSharp",
            "BioSharp+Windows",
            "BioSharp+ImageSharp",
            "BioSharp+ImageMagick"});
            this.FxSelectorComboBox.Margin = new System.Windows.Forms.Padding(1, 0, 6, 0);
            this.FxSelectorComboBox.Name = "FxSelectorComboBox";
            this.FxSelectorComboBox.Size = new System.Drawing.Size(148, 23);
            this.FxSelectorComboBox.ToolTipText = "Select Image Framework";
            this.FxSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.FxSelectorComboBox_SelectedIndexChanged);
            // 
            // FxSelectorLabel
            // 
            this.FxSelectorLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.FxSelectorLabel.AutoSize = false;
            this.FxSelectorLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FxSelectorLabel.Image = global::BiomStudio.Properties.Resources.ic_image_fx_24dp;
            this.FxSelectorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FxSelectorLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FxSelectorLabel.Name = "FxSelectorLabel";
            this.FxSelectorLabel.Size = new System.Drawing.Size(48, 24);
            this.FxSelectorLabel.Text = "fx :";
            this.FxSelectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FxSelectorLabel.ToolTipText = "Select Image Framework ->";
            // 
            // MainButtonSeparator6
            // 
            this.MainButtonSeparator6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MainButtonSeparator6.Name = "MainButtonSeparator6";
            this.MainButtonSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // ImageOpacityTrackBar
            // 
            this.ImageOpacityTrackBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ImageOpacityTrackBar.AutoSize = false;
            this.ImageOpacityTrackBar.Enabled = false;
            this.ImageOpacityTrackBar.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ImageOpacityTrackBar.Maximum = 100;
            this.ImageOpacityTrackBar.Name = "ImageOpacityTrackBar";
            this.ImageOpacityTrackBar.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ImageOpacityTrackBar.Size = new System.Drawing.Size(100, 24);
            this.ImageOpacityTrackBar.TickFrequency = 5;
            this.ImageOpacityTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ImageOpacityTrackBar.Value = 100;
            this.ImageOpacityTrackBar.ValueChanged += new System.EventHandler(this.ImageOpacityTrackBar_ValueChanged);
            this.ImageOpacityTrackBar.MouseLeave += new System.EventHandler(this.ImageOpacityTrackBar_MouseLeave);
            this.ImageOpacityTrackBar.MouseHover += new System.EventHandler(this.ImageOpacityTrackBar_MouseHover);
            this.ImageOpacityTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageOpacityTrackBar_MouseUp);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // ImageAsFingerprintButton
            // 
            this.ImageAsFingerprintButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ImageAsFingerprintButton.Checked = true;
            this.ImageAsFingerprintButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ImageAsFingerprintButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ImageAsFingerprintButton.Image = global::BiomStudio.Properties.Resources.ic_fingerprint_24dp;
            this.ImageAsFingerprintButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImageAsFingerprintButton.Name = "ImageAsFingerprintButton";
            this.ImageAsFingerprintButton.Size = new System.Drawing.Size(28, 28);
            this.ImageAsFingerprintButton.ToolTipText = "Treat all images as biometric prints";
            // 
            // MainContentPanel
            // 
            this.MainContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainContentPanel.BackColor = System.Drawing.Color.White;
            this.MainContentPanel.Controls.Add(this.MainSplitContainer);
            this.MainContentPanel.Location = new System.Drawing.Point(0, 56);
            this.MainContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainContentPanel.Name = "MainContentPanel";
            this.MainContentPanel.Size = new System.Drawing.Size(884, 484);
            this.MainContentPanel.TabIndex = 4;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.MainSplitContainer.Panel1.Controls.Add(this.SettingsTabControl);
            this.MainSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.AutoScroll = true;
            this.MainSplitContainer.Panel2.BackColor = System.Drawing.Color.White;
            this.MainSplitContainer.Panel2.Controls.Add(this.ImagePanel);
            this.MainSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.MainSplitContainer.Size = new System.Drawing.Size(884, 484);
            this.MainSplitContainer.SplitterDistance = 393;
            this.MainSplitContainer.TabIndex = 4;
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.ReadImageTabPage);
            this.SettingsTabControl.Controls.Add(this.WriteImageTabPage);
            this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTabControl.Location = new System.Drawing.Point(3, 3);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(387, 478);
            this.SettingsTabControl.TabIndex = 1;
            // 
            // ReadImageTabPage
            // 
            this.ReadImageTabPage.Controls.Add(this.ReadImagePropertyGrid);
            this.ReadImageTabPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ReadImageTabPage.Location = new System.Drawing.Point(4, 24);
            this.ReadImageTabPage.Name = "ReadImageTabPage";
            this.ReadImageTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ReadImageTabPage.Size = new System.Drawing.Size(379, 450);
            this.ReadImageTabPage.TabIndex = 0;
            this.ReadImageTabPage.Text = "Read Image";
            this.ReadImageTabPage.UseVisualStyleBackColor = true;
            // 
            // ReadImagePropertyGrid
            // 
            this.ReadImagePropertyGrid.BackColor = System.Drawing.Color.White;
            this.ReadImagePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadImagePropertyGrid.HelpBackColor = System.Drawing.Color.White;
            this.ReadImagePropertyGrid.HelpBorderColor = System.Drawing.Color.WhiteSmoke;
            this.ReadImagePropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.ReadImagePropertyGrid.Name = "ReadImagePropertyGrid";
            this.ReadImagePropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.ReadImagePropertyGrid.Size = new System.Drawing.Size(373, 444);
            this.ReadImagePropertyGrid.TabIndex = 1;
            this.ReadImagePropertyGrid.ToolbarVisible = false;
            this.ReadImagePropertyGrid.ViewBackColor = System.Drawing.Color.White;
            this.ReadImagePropertyGrid.ViewBorderColor = System.Drawing.Color.White;
            // 
            // WriteImageTabPage
            // 
            this.WriteImageTabPage.Controls.Add(this.WriteImagePropertyGrid);
            this.WriteImageTabPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WriteImageTabPage.Location = new System.Drawing.Point(4, 24);
            this.WriteImageTabPage.Name = "WriteImageTabPage";
            this.WriteImageTabPage.Size = new System.Drawing.Size(379, 450);
            this.WriteImageTabPage.TabIndex = 1;
            this.WriteImageTabPage.Text = "Write Image";
            this.WriteImageTabPage.UseVisualStyleBackColor = true;
            // 
            // WriteImagePropertyGrid
            // 
            this.WriteImagePropertyGrid.BackColor = System.Drawing.Color.White;
            this.WriteImagePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WriteImagePropertyGrid.HelpBackColor = System.Drawing.Color.White;
            this.WriteImagePropertyGrid.HelpBorderColor = System.Drawing.Color.WhiteSmoke;
            this.WriteImagePropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.WriteImagePropertyGrid.Name = "WriteImagePropertyGrid";
            this.WriteImagePropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.WriteImagePropertyGrid.Size = new System.Drawing.Size(379, 450);
            this.WriteImagePropertyGrid.TabIndex = 2;
            this.WriteImagePropertyGrid.ToolbarVisible = false;
            this.WriteImagePropertyGrid.ViewBackColor = System.Drawing.Color.White;
            this.WriteImagePropertyGrid.ViewBorderColor = System.Drawing.Color.White;
            // 
            // ImagePanel
            // 
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.BackColor = System.Drawing.Color.Transparent;
            this.ImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagePanel.Image = null;
            this.ImagePanel.Location = new System.Drawing.Point(3, 3);
            this.ImagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(481, 478);
            this.ImagePanel.SizeMode = ScrollablePicturePanel.PicturePanelSizeMode.Actual;
            this.ImagePanel.TabIndex = 0;
            this.ImagePanel.ImageChanged += new System.EventHandler(this.ImagePanel_ImageChanged);
            // 
            // FileExit
            // 
            this.FileExit.Button = null;
            this.FileExit.Checked = false;
            this.FileExit.Name = null;
            this.FileExit.PopupMenuItem = null;
            this.FileExit.ToolStripButton = null;
            this.FileExit.ToolStripDropDownItem = null;
            this.FileExit.ToolStripMenuItem = this.FileExitMenuItem;
            this.FileExit.Command += new System.EventHandler(this.OnFileExit);
            // 
            // ImageFileOpenFileDialog
            // 
            this.ImageFileOpenFileDialog.SupportMultiDottedExtensions = true;
            this.ImageFileOpenFileDialog.Title = "Open image file";
            // 
            // ImageFileOpen
            // 
            this.ImageFileOpen.Button = null;
            this.ImageFileOpen.Checked = false;
            this.ImageFileOpen.Name = null;
            this.ImageFileOpen.PopupMenuItem = null;
            this.ImageFileOpen.ToolStripButton = this.FileOpenButton;
            this.ImageFileOpen.ToolStripDropDownItem = null;
            this.ImageFileOpen.ToolStripMenuItem = this.FileOpenMenuItem;
            this.ImageFileOpen.Command += new System.EventHandler(this.OnImageFileOpen);
            // 
            // ImageSizeModeFit
            // 
            this.ImageSizeModeFit.Button = null;
            this.ImageSizeModeFit.Checked = false;
            this.ImageSizeModeFit.Name = null;
            this.ImageSizeModeFit.PopupMenuItem = null;
            this.ImageSizeModeFit.ToolStripButton = this.ViewFitButton;
            this.ImageSizeModeFit.ToolStripDropDownItem = null;
            this.ImageSizeModeFit.ToolStripMenuItem = this.ImageSizeModeFitMenuItem;
            this.ImageSizeModeFit.Command += new System.EventHandler(this.OnImageSizeModeFit);
            this.ImageSizeModeFit.UpdateUI += new System.EventHandler(this.OnImageSizeModeFitUpdateUI);
            // 
            // ImageSizeModeActual
            // 
            this.ImageSizeModeActual.Button = null;
            this.ImageSizeModeActual.Checked = false;
            this.ImageSizeModeActual.Name = null;
            this.ImageSizeModeActual.PopupMenuItem = null;
            this.ImageSizeModeActual.ToolStripButton = this.ViewActualButton;
            this.ImageSizeModeActual.ToolStripDropDownItem = null;
            this.ImageSizeModeActual.ToolStripMenuItem = this.ImageSizeModeActualMenuItem;
            this.ImageSizeModeActual.Command += new System.EventHandler(this.OnImageSizeModeActual);
            this.ImageSizeModeActual.UpdateUI += new System.EventHandler(this.OnImageSizeModeActualUpdateUI);
            // 
            // ImageAsPrint
            // 
            this.ImageAsPrint.Button = null;
            this.ImageAsPrint.Checked = true;
            this.ImageAsPrint.Name = null;
            this.ImageAsPrint.PopupMenuItem = null;
            this.ImageAsPrint.ToolStripButton = this.ImageAsFingerprintButton;
            this.ImageAsPrint.ToolStripDropDownItem = null;
            this.ImageAsPrint.ToolStripMenuItem = null;
            this.ImageAsPrint.Command += new System.EventHandler(this.ImageAsPrint_Command);
            this.ImageAsPrint.UpdateUI += new System.EventHandler(this.ImageAsPrint_UpdateUI);
            // 
            // ImageOpacityTrackBarToolTip
            // 
            this.ImageOpacityTrackBarToolTip.AutomaticDelay = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.MainButtonStrip);
            this.Controls.Add(this.MainContentPanel);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MainMenuMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenuMenuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 534);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.MainMenuMenuStrip.ResumeLayout(false);
            this.MainMenuMenuStrip.PerformLayout();
            this.MainButtonStrip.ResumeLayout(false);
            this.MainButtonStrip.PerformLayout();
            this.MainContentPanel.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.SettingsTabControl.ResumeLayout(false);
            this.ReadImageTabPage.ResumeLayout(false);
            this.WriteImageTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FileExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageFileOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSizeModeFit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSizeModeActual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageAsPrint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip MainStatusStrip;
        private ToolStripStatusLabel CopyrightLabel;
        private MenuStrip MainMenuMenuStrip;
        private ToolStripMenuItem FileMenuItem;
        private ToolStripMenuItem FileOpenMenuItem;
        private ToolStripSeparator MainMenuSeparator0;
        private ToolStripMenuItem FileSaveMenuItem;
        private ToolStripMenuItem FileSaveAsMenuItem;
        private ToolStripSeparator MainMenuSeparator1;
        private ToolStripMenuItem FileExitMenuItem;
        private ToolStripMenuItem EditMenuItem;
        private ToolStripMenuItem EditCopyMenuItem;
        private ToolStripMenuItem EditPasteMenuItem;
        private ToolStripMenuItem MainToolsMenuItem;
        private ToolStripMenuItem ToolsOptionsMenuItem;
        private ToolStripMenuItem MainHelpMenuItem;
        private ToolStripMenuItem HelpAboutMenuItem;
        private ToolStrip MainButtonStrip;
        private ToolStripButton FileOpenButton;
        private ToolStripButton FileSaveButton;
        private ToolStripSeparator MainButtonSeparator0;
        private ToolStripButton EditCopyButton;
        private ToolStripButton EditPasteButton;
        private Panel MainContentPanel;
        private SplitContainer MainSplitContainer;
        private FormCommand FileExit;
        private OpenFileDialog ImageFileOpenFileDialog;
        private FormCommand ImageFileOpen;
        private ScrollablePicturePanel ImagePanel;
        private ToolStripMenuItem ViewMenuItem;
        private ToolStripMenuItem ViewImageSizeModeMenuItem;
        private ToolStripMenuItem ImageSizeModeFitMenuItem;
        private ToolStripMenuItem ImageSizeModeActualMenuItem;
        private FormCommand ImageSizeModeFit;
        private FormCommand ImageSizeModeActual;
        private TabControl SettingsTabControl;
        private TabPage ReadImageTabPage;
        private PropertyGrid ReadImagePropertyGrid;
        private TabPage WriteImageTabPage;
        private PropertyGrid WriteImagePropertyGrid;
        private ToolStripSeparator MainButtonSeparator1;
        private ToolStripButton ViewFitButton;
        private ToolStripButton ViewActualButton;
        private ToolStripSeparator MainButtonSeparator2;
        private ToolStripButton ToolsOptionsButton;
        private ToolStripComboBox FxSelectorComboBox;
        private ToolStripLabel FxSelectorLabel;
        private ToolStripButton ImageAsFingerprintButton;
        private ToolStripSeparator MainButtonSeparator6;
        private FormCommand ImageAsPrint;
        private ToolStripTrackBarItem ImageOpacityTrackBar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolTip ImageOpacityTrackBarToolTip;
    }
}
