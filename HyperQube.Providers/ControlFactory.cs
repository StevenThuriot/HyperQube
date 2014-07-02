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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HyperQube.Library.Questions;
using HyperQube.Providers.ControlChain;

namespace HyperQube.Providers
{
    [Export(typeof (IControlFactory))]
    internal class ControlFactory : IControlFactory
    {
        internal static readonly Size MaximumLabelSize = new Size(135, 23);
        internal static readonly Size MaximumSize = new Size(180, 23);
        internal static readonly Size MinimumSize = new Size(80, 23);
        private readonly IEnumerable<IBuilder> _builders;

        [ImportingConstructor]
        public ControlFactory([ImportMany] IEnumerable<IBuilder> builders)
        {
            _builders = builders;
        }

        public Control[] Build(IEnumerable<IQuestion> questions, ErrorProvider errorProvider)
        {
            //While grouping might be more performant, we would influence the order by doing so.
            //Instead, we'll just do a dumb build.
            return questions.Select(x => Build(x, errorProvider)).ToArray();
        }

        public Control Build(IQuestion question, ErrorProvider errorProvider)
        {
            var control = GetBuilder(question.QuestionType).Build(question, errorProvider);

            control.Tag = question.Tag;

            return control;
        }

        public void MapTo(IEnumerable<Control> controls, IEnumerable<IQuestion> questions)
        {
            var groups = controls.Select(x => Tuple.Create(x, questions.FirstOrDefault(y => Equals(x.Tag, y.Tag))))
                                 .Where(x => x.Item2 != null)
                                 .GroupBy(x => x.Item2.QuestionType);

            foreach (var group in groups)
            {
                var builder = GetBuilder(group.Key);

                foreach (var tuple in group)
                    tuple.Item2.Result = builder.GetValue(tuple.Item1);
            }
        }

        private IBuilder GetBuilder(QuestionType questionType)
        {
            var builder = _builders.FirstOrDefault(x => x.IsCompatible(questionType));

            if (builder == null)
                throw new NotSupportedException("No compatible builder found.");

            return builder;
        }


        public static void DisposeControl(IEnumerable<Control> controls)
        {
            foreach (var control in controls)
                DisposeControl(control);
        }

        public static void DisposeControl(Control control)
        {
            if (control == null) return;

            var panel = control as Panel;
            if (panel != null)
            {
                foreach (var child in panel.Controls.OfType<Control>())
                    DisposeControl(child);
            }

            control.Tag = null;
            control.Dispose();
        }
    }
}
