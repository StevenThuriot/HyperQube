using System.ComponentModel.Composition;
using System.Windows.Forms;
using HyperQube.Library.Questions;

namespace HyperQube.Providers.ControlChain
{
    [InheritedExport]
    public interface IBuilder
    {
        bool IsCompatible(QuestionType type);
        Control Build(IQuestion question, ErrorProvider errorProvider);
        dynamic GetValue(Control control);
    }
}
