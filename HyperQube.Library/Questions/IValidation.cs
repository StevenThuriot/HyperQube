using System;

namespace HyperQube.Library.Questions
{
    public interface IValidation
    {
        Predicate<dynamic> IsValid { get; }

        /// <remarks>{title} can be used to inject the question's title.</remarks>
        string Message { get; }
    }
}
