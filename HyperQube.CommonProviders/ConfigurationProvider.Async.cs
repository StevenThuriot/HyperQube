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

using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace HyperQube.CommonProviders
{
    partial class ConfigurationProvider
    {
        public Task<string> GetValueAsync(string key)
        {
            return Task.Run(() => GetValue(key));
        }

        public Task<IEnumerable<string>> GetValuesAsync(params string[] keys)
        {
            return Task.Run(() => GetValues(keys));
        }

        public Task SetValueAsync(string key, string value)
        {
            return Task.Run(() => SetValue(key, value));
        }

        public Task SetValuesAsync(IDictionary<string, string> kvps)
        {
            return Task.Run(() => SetValues(kvps));
        }

        public Task<SecureString> GetSecureValueAsync(string key)
        {
            return Task.Run(() => GetSecureValue(key));
        }

        public Task SetSecureValueAsync(string key, string value)
        {
            return Task.Run(() => SetSecureValue(key, value));
        }

        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        public Task<string> GetAPIKeyAsync()
        {
            return GetValueAsync(Apikey);
        }

        public Task SetAPIKeyAsync(string value)
        {
            return SetValueAsync(Apikey, value);
        }

        public Task RemoveAsync(string key)
        {
            return Task.Run(() => Remove(key));
        }

        public Task RemoveAsync(params string[] keys)
        {
            return Task.Run(() => Remove(keys));
        }
    }
}