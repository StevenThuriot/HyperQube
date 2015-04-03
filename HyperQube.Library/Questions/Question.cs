using System;

namespace HyperQube.Library.Questions
{
    public abstract class Question<T> : IQuestion
    {
        private readonly T _initialValue;
        private readonly Guid _tag = Guid.NewGuid();

        private readonly string _title;
        private readonly QuestionType _type;
        private readonly bool _isRequired;

        protected Question(string title, QuestionType type = QuestionType.Text, bool isRequired = true, T initialValue = default (T))
        {
            if (title == null)
                throw new ArgumentNullException("title");

            _title = title;
            _initialValue = initialValue;
            _type = type;
            _isRequired = isRequired;
        }

        public virtual T InitialValue
        {
            get { return _initialValue; }
        }

        public T Result { set; get; }

        public bool IsRequired
        {
            get { return _isRequired; }
        }

        public Guid Tag
        {
            get { return _tag; }
        }

        public virtual QuestionType QuestionType
        {
            get { return _type; }
        }

        public virtual string Title
        {
            get { return _title; }
        }

        dynamic IQuestion.InitialValue
        {
            get { return InitialValue; }
        }

        dynamic IQuestion.Result
        {
            set { Result = value; }
        }
    }
}
