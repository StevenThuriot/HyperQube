using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HyperQube.Library.Questions;

namespace HyperQube.Providers.ControlChain
{
    internal static class ValidationBuilder
    {
        public static void Build(IQuestion question, ErrorProvider errorProvider, Control control,
                                 Func<Control, dynamic> itemSelector, Predicate<dynamic> isRequired = null,
                                 int padding = -20)
        {
            var validatableQuestion = question as IValidatableQuestion;

            var validations = validatableQuestion == null
                                  ? new List<IValidation>()
                                  : validatableQuestion.Validations.ToList();

            if (question.IsRequired)
            {
                var validation = new RequiredValidation(isRequired ?? (x => x != null));
                validations.Insert(0, validation);
            }


            if (validations.Count == 0) return;

            ConfigureValidation(question, control, errorProvider, itemSelector, validations, padding);
        }

        private static void ConfigureValidation(IQuestion question, Control control, ErrorProvider provider,
                                                Func<Control, dynamic> itemSelector,
                                                IEnumerable<IValidation> validations, int padding)
        {
            provider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
            provider.SetIconPadding(control, padding);
            provider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            provider.BlinkRate = int.MaxValue;

            var margin = control.Margin;
            control.Margin = new Padding(margin.Left, margin.Top, margin.Right + 25, margin.Bottom);

            var title = question.Title;

            control.Validating += (sender, args) =>
                                  {
                                      var senderControl = (Control) sender;

                                      var item = itemSelector(senderControl);

                                      var validation = validations.FirstOrDefault(x => !x.IsValid(item));
                                      if (validation == null) return;

                                      args.Cancel = true;

                                      string value = item == null ? "{Null}" : (item as string ?? Convert.ToString(item));

                                      var errorMessage = validation.Message.Replace("{title}", title)
                                                                           .Replace("{value}", value);
                                      
                                      provider.SetError(senderControl, errorMessage);
                                  };

            control.Validated += (sender, args) => provider.SetError((Control) sender, "");
        }
    }
}
