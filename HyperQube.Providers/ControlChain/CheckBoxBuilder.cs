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
using System.Windows.Forms;
using MetroFramework.Controls;
using HyperQube.Library.Questions;

namespace HyperQube.Providers.ControlChain
{
    internal class CheckBoxBuilder : IBuilder
    {
        public bool IsCompatible(QuestionType type)
        {
            return type == QuestionType.Checkable || type == QuestionType.Togglable;
        }

        public Control Build(IQuestion question, ErrorProvider errorProvider)
        {
            Control returnControl;
            CheckBox box;

            if (question.QuestionType == QuestionType.Checkable)
            {
                returnControl = box = new MetroCheckBox();
                box.Margin = new Padding(22, 3, 3, 3);
                var text = question.Title;

                using (var graphics = box.CreateGraphics())
                {
                    var size = graphics.MeasureString(text, box.Font);
                    box.Width = (int)Math.Ceiling(size.Width + 45); //Add room for the actual checkbox.
                }

                box.Text = text;
            }
            else if (question.QuestionType == QuestionType.Togglable)
            {
                box = new MetroToggle();
                returnControl = LabelBuilder.Build(question, box);
            }
            else
            {
                throw new NotSupportedException(question.QuestionType.ToString());
            }

            box.Checked = question.InitialValue;
            box.ThreeState = false;

            ValidationBuilder.Build(question, errorProvider, box, c => ((CheckBox) c).Checked, padding: 2);


            return returnControl;
        }

        public dynamic GetValue(Control control)
        {
            return LabelBuilder.GetPropertyValue(control, c => ((CheckBox)c).Checked);
        }
    }
}
