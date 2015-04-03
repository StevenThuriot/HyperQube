using System.Windows.Forms;
using HyperQube.Library.Questions;
using MetroFramework.Controls;

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
