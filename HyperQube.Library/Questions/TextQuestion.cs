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

namespace HyperQube.Library.Questions
{
    public class TextQuestion : Question<string>
    {
        public TextQuestion(string title, bool isRequired = true, string initialValue = "", QuestionType type = QuestionType.Text)
            : base(title, type, isRequired, initialValue)
        {
            if (type != QuestionType.Text && type != QuestionType.Multiline && type != QuestionType.Password)
                throw new NotSupportedException(type.ToString());
        }
    }
}
