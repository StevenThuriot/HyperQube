#if DEBUG

using System.ComponentModel.Composition;
using HyperQube.Library;
using HyperQube.Library.Extensions;

namespace HyperQube
{
    class OutputForwarder : IQube
    {
        private readonly IOutputProvider _outputProvider;

        [ImportingConstructor]
        public OutputForwarder(IOutputProvider outputProvider)
        {
            _outputProvider = outputProvider;
        }

        public string Title
        {
            get { return "Output Forwarder"; }
        }

        public Interests Interests
        {
            get { return Interests.All; }
        }

        public void Receive(dynamic json)
        {
            var body = Push.GetBody((object) json);
            if (body == null) return;

            if (json.title == null) return;

            var title = "HyperQube - " + json.title.ToString();
            string bodyString = body.ToString();

            _outputProvider.Write(title, bodyString);
        }
    }
}

#endif