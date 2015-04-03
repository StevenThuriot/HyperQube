namespace HyperQube.Library
{
    public interface IOutputProvider
    {
        void Write(string title, string message, QubeIcon icon = QubeIcon.Info);


        void Trace(string message);
        void Trace(string message, params object[] args);
        void TraceError(string message);
        void TraceError(string message, params object[] args);
    }
}
