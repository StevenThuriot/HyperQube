using HyperQube.Library.Questions;

namespace HyperQube.Library
{
    public interface IInputProvider
    {
        bool Ask(string title, string subtitle, params IQuestion[] questions);
    }
}
