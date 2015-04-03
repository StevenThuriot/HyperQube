using System;
using System.Diagnostics.Contracts;

namespace HyperQube.Library.Questions
{
    [Pure]
    public class Validation : IValidation
    {
        public Validation(Predicate<dynamic> isValid, string message)
        {
            if (isValid == null)
                throw new ArgumentNullException("isValid");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");

            IsValid = isValid;
            Message = message;
        }

        public Predicate<dynamic> IsValid { get; private set; }
        public string Message { get; private set; }
    }
}
