using System;
using System.Collections;
using System.Collections.Generic;

namespace HyperQube.Library.Questions
{
    public class SelectableQuestion : Question<object>, IQuestionWithItems
    {
        private readonly IEnumerable _items;

        public SelectableQuestion(string title, IEnumerable items, bool isRequired = true, object initialValue = null)
            : base(title, QuestionType.Selectable, isRequired, initialValue)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            _items = items;
        }
        
        public IEnumerable Items
        {
            get { return _items; }
        }
    }

    public class SelectableQuestion<T> : Question<T>, IQuestionWithItems
    {
        private readonly IEnumerable<T> _items;

        public SelectableQuestion(string title, IEnumerable<T> items, bool isRequired = true, T initialValue = default(T))
            : base(title, QuestionType.Selectable, isRequired, initialValue)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            _items = items;
        }

        public IEnumerable<T> Items
        {
            get { return _items; }
        }

        IEnumerable IQuestionWithItems.Items
        {
            get { return _items; }
        }
    }
}
