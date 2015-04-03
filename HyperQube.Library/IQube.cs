using System.ComponentModel.Composition;

namespace HyperQube.Library
{
    [InheritedExport]
    public interface IQube
    {
        string Title { get; }

        Interests Interests { get; }

        void Receive(dynamic json);
    }
}
