using System;

namespace HyperQube.Library.Questions
{
    public class BooleanQuestion : Question<bool>
    {
        public BooleanQuestion(string title, bool initialValue = true, QuestionType type = QuestionType.Checkable)
            : base(title, type, initialValue: initialValue, isRequired: false)
        {
            if (type != QuestionType.Checkable && type != QuestionType.Togglable)
                throw new NotSupportedException(type.ToString());
        }
    }
}
