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