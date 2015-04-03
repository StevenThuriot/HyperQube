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
