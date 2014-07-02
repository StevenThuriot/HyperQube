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
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using HyperQube.Library.Questions;

namespace HyperQube.Providers.ControlChain
{
    internal class ComboBoxBuilder : IBuilder
    {
        public bool IsCompatible(QuestionType type)
        {
            return type == QuestionType.Selectable;
        }

        public Control Build(IQuestion question, ErrorProvider errorProvider)
        {
            var questionWithItems = question as IQuestionWithItems;
            if (questionWithItems == null)
                throw new NotSupportedException("question does not implement IQuestionWithItems.");

            var comboBox = new MetroComboBox
                           {
                               Width = ControlFactory.MaximumSize.Width,
                               Anchor = AnchorStyles.Bottom,
                               DropDownStyle = ComboBoxStyle.DropDownList,
                           };

            var items = questionWithItems.Items.Cast<object>().ToArray();
            comboBox.Items.AddRange(items);

            comboBox.SelectedItem = question.InitialValue;

            ValidationBuilder.Build(question, errorProvider, comboBox, c => ((ComboBox) c).SelectedItem, x => x != null, padding: 2);

            return LabelBuilder.Build(question, comboBox);
        }

        public dynamic GetValue(Control control)
        {
            return LabelBuilder.GetPropertyValue(control, c => ((ComboBox)c).SelectedItem);
        }
    }
}
