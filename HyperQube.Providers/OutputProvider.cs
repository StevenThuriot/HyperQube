using System.ComponentModel.Composition;
using HyperQube.Library;

namespace HyperQube.Providers
{
    [Export(typeof (IOutputProvider))]
    internal class OutputProvider : IOutputProvider
    {
        private readonly ITooltipProvider _tooltipProvider;
        private readonly IBulletTrace _trace;

        [ImportingConstructor]
        public OutputProvider(ITooltipProvider tooltipProvider, IBulletTrace trace)
        {
            _tooltipProvider = tooltipProvider;
            _trace = trace;
        }

        public void Write(string title, string message, QubeIcon icon)
        {
            _tooltipProvider.ShowTooltip(title, message, icon);
        }

        public void Trace(string message)
        {
            _trace.Information(message);
        }

        public void Trace(string message, params object[] args)
        {
            _trace.Information(message, args);
        }

        public void TraceError(string message)
        {
            _trace.Error(message);
        }

        public void TraceError(string message, params object[] args)
        {
            _trace.Error(message, args);
        }
    }
}
