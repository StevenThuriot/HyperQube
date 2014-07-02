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