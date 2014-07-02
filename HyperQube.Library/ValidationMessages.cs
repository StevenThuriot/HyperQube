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
    public static class ValidationMessages
    {
        public const string IsRequired = "{title} is a required field.";
        public const string InvalidFormat = "Invalid {title} format.";
        public const string InvalidUri = "'{value}' is not a valid Uri.";
        public const string NumbersOnly = "{title} can only contain numbers.";
        public const string InvalidPort = "'{value}' is not a valid port number.";
        public const string Whitespace = "Whitespace is not allowed in the '{title}' field.";
        public const string LineBreaks = "Line breaks are not allowed in the '{title}' field.";
    }
}