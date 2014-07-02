#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using HyperQube.Library.Questions;

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
