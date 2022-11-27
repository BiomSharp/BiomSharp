// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace BiomStudio.Controls
{
    [DesignerCategory("code")]
    [ToolStripItemDesignerAvailability(
        ToolStripItemDesignerAvailability.ContextMenuStrip
        | ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class ToolStripTrackBarItem : ToolStripControlHost
    {
        private readonly TrackBar trackBar;

        public ToolStripTrackBarItem() : base(new TrackBar() { AutoSize = false })
            => trackBar = (Control as TrackBar)!;

        [DefaultValue(0)]
        public int Value
        {
            get => trackBar.Value;
            set => trackBar.Value = value;
        }

        [DefaultValue(0)]
        public int Minimum
        {
            get => trackBar.Minimum;
            set => trackBar.Minimum = value;
        }

        [DefaultValue(10)]
        public int Maximum
        {
            get => trackBar.Maximum;
            set => trackBar.Maximum = value;
        }

        [DefaultValue(TickStyle.BottomRight)]
        public TickStyle TickStyle
        {
            get => trackBar.TickStyle;
            set => trackBar.TickStyle = value;
        }

        [DefaultValue(1)]
        public int TickFrequency
        {
            get => trackBar.TickFrequency;
            set => trackBar.TickFrequency = value;
        }

        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            (control as TrackBar)!.ValueChanged += new EventHandler(OnTrackBarValueChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            (control as TrackBar)!.ValueChanged -= new EventHandler(OnTrackBarValueChanged);
        }

        private void OnTrackBarValueChanged(object? sender, EventArgs e) =>
            ValueChanged?.Invoke(sender, e);


        public event EventHandler? ValueChanged;

        protected override Size DefaultSize => new(200, 16);
    }
}
