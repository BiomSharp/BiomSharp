// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

namespace BiomStudio.Controls
{
    public class CommandSourceEventArgs : EventArgs
    {
        public object? Source { get; set; }
    }

    public class FormCommand : Component, ISupportInitialize
    {
        public FormCommand() { }

        public FormCommand(
            EventHandler preCommand,
            EventHandler command,
            EventHandler updateUI,
            ButtonBase button,
            ToolStripButton toolStripButton,
            ToolStripMenuItem toolStripMenuItem,
            ToolStripMenuItem popupMenuItem,
            ToolStripDropDownItem toolStripDropDownItem)
        {
            PreCommand = preCommand;
            Command = command;
            UpdateUI = updateUI;
            Button = button;
            ToolStripButton = toolStripButton;
            ToolStripMenuItem = toolStripMenuItem;
            PopupMenuItem = popupMenuItem;
            ToolStripDropDownItem = toolStripDropDownItem;
            if (Command == null)
            {
                throw new InvalidOperationException(
                    "This FormCommand ctor requires Command != null");
            }
            AttachCommand();
        }

        private void OnIdle(object? sender, EventArgs e) => UpdateUI?.Invoke(this, EventArgs.Empty);

        public void ForceUpdateUI() => OnIdle(null, EventArgs.Empty);

        [ReadOnly(true)]
        public bool Attached { get; private set; }

        [ReadOnly(true)]
        public bool Active { get; private set; }

        [ReadOnly(true)]
        public bool Enabled { get; private set; }

        private bool @checked;
        [ReadOnly(false)]
        public bool Checked
        {
            get => @checked;
            set
            {
                @checked = value;
                SetChecked(@checked);
            }
        }

        public string? Name { get; set; }

        public void RunCommand() => OnCommand(null, EventArgs.Empty);

        private void OnCommand(object? sender, EventArgs e)
        {
            if (!Active)
            {
                Active = true;
                PreCommand?.Invoke(this,
                    new CommandSourceEventArgs() { Source = sender });
                Command?.Invoke(this,
                    new CommandSourceEventArgs() { Source = sender });
                Active = false;
            }
        }

        private void SetChecked(bool checkState)
        {
            if (Button is not null and CheckBox box)
            {
                box.Checked = checkState;
            }
            if (ToolStripButton != null)
            {
                ToolStripButton.Checked = checkState;
            }
            if (ToolStripMenuItem != null)
            {
                ToolStripMenuItem.Checked = checkState;
            }
            if (PopupMenuItem != null)
            {
                PopupMenuItem.Checked = checkState;
            }
        }

        private bool GetChecked(object? sender)
        {
            if (ReferenceEquals(sender, Button))
            {
                if (Button is CheckBox box)
                {
                    return box.Checked;
                }
            }
            else if (ReferenceEquals(sender, ToolStripButton))
            {
                return ToolStripButton?.Checked ?? false;
            }
            else if (ReferenceEquals(sender, ToolStripMenuItem))
            {
                return ToolStripMenuItem?.Checked ?? false;
            }
            else if (ReferenceEquals(sender, PopupMenuItem))
            {
                return PopupMenuItem?.Checked ?? false;
            }
            return false;
        }

        private void OnCheckChanged(object? sender, EventArgs e)
        {
            @checked = GetChecked(sender);
            SetChecked(@checked);
        }

        public void DefaultUpdateUI(bool enabled)
        {
            Enabled = enabled;
            if (Button != null && Button.Enabled != Enabled)
            {
                Button.Enabled = Enabled;
            }

            if (ToolStripButton != null && ToolStripButton.Enabled != Enabled)
            {
                ToolStripButton.Enabled = Enabled;
            }

            if (ToolStripMenuItem != null && ToolStripMenuItem.Enabled != Enabled)
            {
                ToolStripMenuItem.Enabled = Enabled;
            }

            if (PopupMenuItem != null && PopupMenuItem.Enabled != Enabled)
            {
                PopupMenuItem.Enabled = Enabled;
            }

            if (ToolStripDropDownItem != null && ToolStripDropDownItem.Enabled != Enabled)
            {
                ToolStripDropDownItem.Enabled = Enabled;
            }
        }

        private void OnCommandDisposed(object? sender, EventArgs e)
        {
            IsDisposed = true;
            DetachCommand();
        }

        private void AttachCommand()
        {
            if (ToolStripMenuItem != null)
            {
                ToolStripMenuItem.CheckedChanged += OnCheckChanged;
                ToolStripMenuItem.Click += OnCommand;
                ToolStripMenuItem.Disposed += OnCommandDisposed;
            }
            if (PopupMenuItem != null)
            {
                PopupMenuItem.CheckedChanged += OnCheckChanged;
                PopupMenuItem.Click += OnCommand;
                PopupMenuItem.Disposed += OnCommandDisposed;
            }
            if (Button != null)
            {
                if (Button is CheckBox box)
                {
                    box.CheckedChanged += OnCheckChanged;
                }
                Button.Click += OnCommand;
                Button.Disposed += OnCommandDisposed;
            }
            if (ToolStripButton != null)
            {
                ToolStripButton.CheckedChanged += OnCheckChanged;
                ToolStripButton.Click += OnCommand;
                ToolStripButton.Disposed += OnCommandDisposed;
            }
            if (ToolStripDropDownItem != null)
            {
                ToolStripDropDownItem.Click += OnCommand;
                ToolStripDropDownItem.Disposed += OnCommandDisposed;
            }
            Application.Idle += OnIdle;
            Attached = true;
        }

        private void DetachCommand()
        {
            if (Command != null)
            {
                Application.Idle -= OnIdle;
                if (ToolStripMenuItem != null)
                {
                    ToolStripMenuItem.Click -= OnCommand;
                    ToolStripMenuItem.CheckedChanged -= OnCheckChanged;
                    ToolStripMenuItem.Disposed -= OnCommandDisposed;
                }
                if (PopupMenuItem != null)
                {
                    PopupMenuItem.Click -= OnCommand;
                    PopupMenuItem.CheckedChanged -= OnCheckChanged;
                    PopupMenuItem.Disposed -= OnCommandDisposed;
                }
                if (Button != null)
                {
                    if (Button is CheckBox box)
                    {
                        box.CheckedChanged -= OnCheckChanged;
                    }
                    Button.Click -= OnCommand;
                    Button.Disposed -= OnCommandDisposed;
                }
                if (ToolStripButton != null)
                {
                    ToolStripButton.Click -= OnCommand;
                    ToolStripButton.CheckedChanged -= OnCheckChanged;
                    ToolStripButton.Disposed -= OnCommandDisposed;
                }
                if (ToolStripDropDownItem != null)
                {
                    ToolStripDropDownItem.Click -= OnCommand;
                    ToolStripDropDownItem.Disposed -= OnCommandDisposed;
                }
            }
            Attached = false;
        }

        [Description("Toolstrip menu item control.")]
        [Category("UI Controls")]
        public ToolStripMenuItem? ToolStripMenuItem { get; set; }

        [Description("Popup menu item control.")]
        [Category("UI Controls")]
        public ToolStripMenuItem? PopupMenuItem { get; set; }

        [Description("Form button item control.")]
        [Category("UI Controls")]
        public ButtonBase? Button { get; set; }

        [Description("Tool strip button item control.")]
        [Category("UI Controls")]
        public ToolStripButton? ToolStripButton { get; set; }

        [Description("Tool strip drop-down item control.")]
        [Category("UI Controls")]
        public ToolStripDropDownItem? ToolStripDropDownItem { get; set; }

        [Description(
            "Pre-command event handler. Event when any" +
            " of the UI controls require pre-command execution.")]
        [Category("Behavior")]
        public event EventHandler PreCommand = default!;

        [Description(
            "Command event handler. Event when any" +
            " of the UI controls 'Click' event occurs.")]
        [Category("Behavior")]
        public event EventHandler Command = default!;

        [Description(
            "UI update event handler. All UI controls can be" +
            " enabled or disabled in this handler. Event occurs" + "" +
            " when the Application idle routine runs.")]
        [Category("Behavior")]
        public event EventHandler UpdateUI = default!;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDisposed { get; private set; }

        #region ISupportInitialize implementation.

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            if (!Attached)
            {
                AttachCommand();
            }
        }

        #endregion ISupportInitialize implementation.

        // Override base to detach the command.
        protected override void Dispose(bool disposing)
        {
            if (Attached)
            {
                DetachCommand();
            }
            base.Dispose(disposing);
        }
    }
}
