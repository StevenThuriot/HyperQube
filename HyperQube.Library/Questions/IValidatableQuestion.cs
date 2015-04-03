using System.Collections.Generic;

namespace HyperQube.Library.Questions
{
    public interface IValidatableQuestion : IQuestion
    {
        /// <summary>
        ///     Custom validations for a question field.
        /// </summary>
        IEnumerable<IValidation> Validations { get; }
    }
}
