using System;
using System.Linq;
using System.Windows.Forms;
using HyperQube.Library.Questions;
using MetroFramework.Controls;

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
