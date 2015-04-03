using System.Collections;

namespace HyperQube.Library.Questions
{
    public interface IQuestionWithItems : IQuestion
    {
        IEnumerable Items { get; }
    }
}
