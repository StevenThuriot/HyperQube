using System;
using System.Collections.Generic;
using System.Linq;
using HyperQube.Library;
using HyperQube.Library.Questions;

namespace HyperQube
{
    class ApiQuestion : TextQuestion, IValidatableQuestion
    {
        public ApiQuestion()
            : base("API Key", true, "", QuestionType.Multiline)
        {
            var noLinebreaks = new Validation(x => !((string)x).Contains(Environment.NewLine), ValidationMessages.LineBreaks);
            var noWhitespace = new Validation(x => !((string) x).Any(char.IsWhiteSpace), ValidationMessages.Whitespace);

            Validations = new[] { noLinebreaks, noWhitespace };
        }

        public IEnumerable<IValidation> Validations { get; private set; }
    }
}