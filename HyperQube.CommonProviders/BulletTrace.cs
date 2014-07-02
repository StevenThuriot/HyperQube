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
