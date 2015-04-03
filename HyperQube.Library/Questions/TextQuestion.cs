using System;

namespace HyperQube.Library.Questions
{
    public class TextQuestion : Question<string>
    {
        public TextQuestion(string title, bool isRequired = true, string initialValue = "", QuestionType type = QuestionType.Text)
            : base(title, type, isRequired, initialValue)
        {
            if (type != QuestionType.Text && type != QuestionType.Multiline && type != QuestionType.Password)
                throw new NotSupportedException(type.ToString());
        }
    }
}
