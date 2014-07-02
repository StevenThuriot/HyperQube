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

using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using HyperQube.Library;
using HyperQube.Library.Questions;

namespace HyperQube.Providers
{
    [Export(typeof (IInputProvider))]
    internal class InputProvider : IInputProvider
    {
        private readonly IControlFactory _factory;

        [ImportingConstructor]
        public InputProvider(IControlFactory factory)
        {
            _factory = factory;
        }

        public bool Ask(string title, string subtitle, params IQuestion[] questions)
        {
            using (
                var stream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("HyperQube.Providers.Resources.Error.png"))
            {
                Debug.Assert(stream != null, "stream != null");
                using (var image = new Bitmap(stream))
                {
                    var pIcon = image.GetHicon();
                    var provider = new ErrorProvider
                                   {
                                       BlinkStyle = ErrorBlinkStyle.NeverBlink,
                                       BlinkRate = int.MaxValue,
                                       Icon = Icon.FromHandle(pIcon)
                                   };

                    using (provider)
                    {
                        var controls = _factory.Build(questions, provider);

                        using (var window = new HosterWindow(title, subtitle, controls, provider))
                        {
                            var result = window.ShowDialog() == DialogResult.OK;

                            if (result) _factory.MapTo(controls, questions);

                            ControlFactory.DisposeControl(controls);
                            return result;
                        }
                    }
                }
            }
        }
    }
}
