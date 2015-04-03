using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HyperQube.Library.Questions;
using MetroFramework;

namespace HyperQube.Providers.ControlChain
{
    internal static class LabelBuilder
    {
        public static dynamic GetPropertyValue<T>(Control control, Func<Control, T> getValue)
        {
            if (control == null) return null;

            return control is Panel
                       ? getValue(control.Controls[1])
                       : getValue(control);
        }

        public static TableLayoutPanel Build(IQuestion question, Control actualControl)
        {
            actualControl.Dock = DockStyle.Left;
            var labelWidth = ControlFactory.MaximumLabelSize.Width;

            var label = new Label
                        {
                            AutoSize = false,
                            Font = MetroFonts.Link(MetroLinkSize.Small, MetroLinkWeight.Regular),
                            Anchor = AnchorStyles.Top | AnchorStyles.Left,
                            Width = labelWidth,
                            TextAlign = ContentAlignment.MiddleLeft,
                            TabStop = false
                        };

            var text = question.Title;
            if (text.Last() != ':') text += ':';

            label.Text = text;

            var layoutPanel = new TableLayoutPanel
                              {
                                  AutoSize = true,
                                  AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                  Dock = DockStyle.Top,
                                  Anchor = AnchorStyles.Top | AnchorStyles.Left,
                                  ColumnCount = 2,
                                  RowCount = 1,
                                  Margin = new Padding(11, 0, 0, 0),
                                  Padding = new Padding(5)
                              };

            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, labelWidth + 5));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            layoutPanel.Controls.Add(label);
            layoutPanel.Controls.Add(actualControl);

            return layoutPanel;
        }
    }
}
