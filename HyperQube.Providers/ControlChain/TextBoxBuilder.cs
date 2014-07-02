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

using System.Windows.Forms;
using MetroFramework.Controls;
using HyperQube.Library.Questions;

namespace HyperQube.Providers.ControlChain
{
    internal class TextBoxBuilder : IBuilder
    {
        public bool IsCompatible(QuestionType type)
        {
            return type == QuestionType.Text || type == QuestionType.Multiline || type == QuestionType.Password;
        }

        public Control Build(IQuestion question, ErrorProvider errorProvider)
        {
            var textBox = new MetroTextBox
                          {
                              Text = question.InitialValue,
                              Width = ControlFactory.MaximumSize.Width,
                              Anchor = AnchorStyles.Bottom
                          };

            switch (question.QuestionType)
            {
                case QuestionType.Multiline:
                    textBox.Multiline = true;
                    textBox.Height = 23*3;
                    break;
                case QuestionType.Password:
                    textBox.UseSystemPasswordChar = true;
                    break;
            }

            ValidationBuilder.Build(question, errorProvider, textBox, c => c.Text, x => !string.IsNullOrEmpty((string)x));

            return LabelBuilder.Build(question, textBox);
        }

        public dynamic GetValue(Control control)
        {
            return LabelBuilder.GetPropertyValue(control, c => c.Text);
        }
    }
}
