using System.Collections.Generic;
using System.Windows.Forms;
using HyperQube.Library.Questions;

namespace HyperQube.Providers
{
    interface IControlFactory
    {
        Control[] Build(IEnumerable<IQuestion> questions, ErrorProvider errorProvider);
        Control Build(IQuestion question, ErrorProvider errorProvider);
        void MapTo(IEnumerable<Control> controls, IEnumerable<IQuestion> questions);
    }
}
