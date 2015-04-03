using System;

namespace HyperQube.Library.Questions
{
    public class RequiredValidation : Validation
    {
        public RequiredValidation()
            : this(x => x != null)
        {
        }

        public RequiredValidation(Predicate<dynamic> isFilledIn)
            : base(isFilledIn, ValidationMessages.IsRequired)
        {
        }
    }
}
