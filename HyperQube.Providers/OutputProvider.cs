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
