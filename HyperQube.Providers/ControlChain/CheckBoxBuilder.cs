using System;
using System.Windows.Forms;
using HyperQube.Library.Questions;
using MetroFramework.Controls;

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
