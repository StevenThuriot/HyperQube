namespace HyperQube.Library
{
    public interface IBulletTrace
    {
        void Information(string message);
        void Information(string message, params object[] args);

        void Error(string message);
        void Error(string message, params object[] args);
    }
}
