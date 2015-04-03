using System;

namespace HyperQube.Library.Questions
{
    public interface IQuestion
    {
        QuestionType QuestionType { get; }
        string Title { get; }
        dynamic InitialValue { get; }
        dynamic Result { set; }

        bool IsRequired { get; }
        Guid Tag { get; }
    }
}
