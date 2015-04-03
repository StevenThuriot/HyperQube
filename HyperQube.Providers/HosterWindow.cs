using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using MetroMessageBox = MetroFramework.MetroMessageBox;

namespace HyperQube.Providers
{
    sealed class HosterWindow : MetroForm
    {
        private MetroButton _cancelButton;
        private MetroLabel _subTitleLabel;
        private FlowLayoutPanel _layoutPanel;
        private MetroButton _saveButton;
        private bool _disposed;
        private ErrorProvider _errorProvider;

        public HosterWindow(string title, string subtitle, Control[] controls, ErrorProvider errorProvider)
        {
            _errorProvider = errorProvider;
            _errorProvider.ContainerControl = this;

            InitializeComponent(title, subtitle, controls);
            ValidateChildren();
        }

        private void InitializeComponent(string title, string subtitle, Control[] controls)
        {
            _saveButton = new MetroButton();
            _cancelButton = new MetroButton();
            _subTitleLabel = new MetroLabel();
            _layoutPanel = new FlowLayoutPanel();

            SuspendLayout();
            // 
            // _LayoutPanel
            // 
            _layoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _layoutPanel.Size = new Size(540, 217);
            _layoutPanel.AutoScroll = true;
            _layoutPanel.FlowDirection = FlowDirection.TopDown;
            _layoutPanel.Location = new Point(36, 80);
            _layoutPanel.Name = "_layoutPanel";
            _layoutPanel.TabIndex = 0;
            _layoutPanel.WrapContents = false;
            _layoutPanel.Controls.AddRange(controls);
            // 
            // _SaveButton
            // 
            _saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _saveButton.Highlight = false;
            _saveButton.Location = new Point(421, 310);
            _saveButton.Name = "_saveButton";
            _saveButton.Size = new Size(75, 23);
            _saveButton.Style = MetroColorStyle.Blue;
            _saveButton.StyleManager = null;
            _saveButton.TabIndex = 1;
            _saveButton.Text = "Save";
            _saveButton.Theme = MetroThemeStyle.Light;
            _saveButton.Click += (sender, args) =>
                                 {
                                     var form = (HosterWindow) ((Control) sender).FindForm();
                                     if (form == null) return;

                                     if (form.ValidateChildren())
                                         form.DialogResult = DialogResult.OK;
                                     else
                                     {
                                         form.ShowErrorMessage();
                                     }
                                 };
            // 
            // _CancelButton
            // 
            _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Highlight = false;
            _cancelButton.Location = new Point(502, 310);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(75, 23);
            _cancelButton.Style = MetroColorStyle.Blue;
            _cancelButton.StyleManager = null;
            _cancelButton.TabIndex = 2;
            _cancelButton.Text = "Cancel";
            _cancelButton.Theme = MetroThemeStyle.Light;
            _cancelButton.Click += (sender, args) =>
                                   {
                                       var form = ((Control) sender).FindForm();
                                       if (form != null) form.DialogResult = DialogResult.Cancel;
                                   };
            // 
            // _SubTitleLabel
            // 
            _subTitleLabel.AutoSize = true;
            _subTitleLabel.FontSize = MetroLabelSize.Medium;
            _subTitleLabel.FontWeight = MetroLabelWeight.Light;
            _subTitleLabel.LabelMode = MetroLabelMode.Default;
            _subTitleLabel.Location = new Point(24, 54);
            _subTitleLabel.Name = "_subTitleLabel";
            _subTitleLabel.Size = new Size(140, 19);
            _subTitleLabel.Style = MetroColorStyle.Blue;
            _subTitleLabel.StyleManager = null;
            _subTitleLabel.TabStop = false;
            _subTitleLabel.Text = subtitle;
            _subTitleLabel.Theme = MetroThemeStyle.Light;
            _subTitleLabel.UseStyleColors = false;
            // 
            // SettingsWindow
            // 
            AcceptButton = _saveButton;
            CancelButton = _cancelButton;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 356);
            MinimumSize = new Size(400, 230);
            Location = new Point(0, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            ShadowType = MetroFormShadowType.None;
            BorderStyle = MetroFormBorderStyle.FixedSingle;
            Text = title;
            AutoValidate = AutoValidate.EnableAllowFocusChange;

            Controls.Add(_layoutPanel);
            Controls.Add(_subTitleLabel);
            Controls.Add(_cancelButton);
            Controls.Add(_saveButton);

            var executingAssembly = Assembly.GetEntryAssembly();
            using (var stream = executingAssembly.GetManifestResourceStream("HyperQube.HyperQube.ico"))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Icon = new Icon(stream);
            }

            ResumeLayout(false);
            PerformLayout();
        }

        public IEnumerable<Control> GetAllControls()
        {
            return GetAllControls(_layoutPanel);
        }

        public static IEnumerable<Control> GetAllControls(Control parent)
        {
            yield return parent;

            foreach (var control in parent.Controls.OfType<Control>().SelectMany(GetAllControls))
                yield return control;
        }

        public IEnumerable<string> GetAllErrorMessages()
        {
            return GetAllControls().Select(control => _errorProvider.GetError(control))
                                   .Where(message => !string.IsNullOrEmpty(message));
        }

        private void ShowErrorMessage()
        {
            var messageList = GetAllErrorMessages().Select(x => "• " + x);
            var messages = string.Join(Environment.NewLine, messageList);

            MetroMessageBox.Show(this, messages, Text + " - Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Icon.Dispose();
                _errorProvider.ContainerControl = null;
                _errorProvider = null;

                _layoutPanel.Controls.Clear();
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
