#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

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
