using System.ComponentModel.Composition;
using System.Diagnostics;
using HyperQube.Library;

namespace HyperQube.CommonProviders
{
    [Export(typeof (IBulletTrace))]
    internal class BulletTrace : IBulletTrace
    {
        private static readonly TraceSource _trace = new TraceSource("HyperQube", SourceLevels.Information);

        public void Information(string message)
        {
            _trace.TraceInformation(message);
        }

        public void Information(string message, params object[] args)
        {
            _trace.TraceInformation(message, args);
        }

        public void Error(string message)
        {
            _trace.TraceEvent(TraceEventType.Error, 0, message);
        }

        public void Error(string message, params object[] args)
        {
            _trace.TraceEvent(TraceEventType.Error, 0, message, args);
        }
    }
}
