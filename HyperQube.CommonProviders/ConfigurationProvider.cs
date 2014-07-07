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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using HyperQube.Library;

namespace HyperQube.CommonProviders
{
    [Export(typeof (IConfigurationProvider))]
    partial class ConfigurationProvider : IDisposable, IConfigurationProvider
    {
        private const string Apikey = "APIKey";
        
        private readonly Configuration _config;
        private bool _disposed;

        public ConfigurationProvider()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetAPIKey()
        {
            return GetValue(Apikey);
        }

        public void SetAPIKey(string value)
        {
            SetValue(Apikey, value);
        }

        public string GetValue(string key)
        {
            var element = GetElement(key);
            return element == null ? null : element.Value;
        }

        public IDictionary<string, string> GetValues(params string[] keys)
        {
            return new ReadOnlyDictionary<string, string>(keys.ToDictionary(x => x, GetValue));
        }

        public void SetValue(string key, string value)
        {
            var element = GetElement(key);
            if (element == null)
                AddElement(key, value);
            else
                element.Value = value;
        }

        public void SetValues(IDictionary<string, string> kvps)
        {
            foreach (var kvp in kvps)
                SetValue(kvp.Key, kvp.Value);
        }

        private const string Entropy = "~#HyperQube#~";
        public SecureString GetSecureValue(string key)
        {
            var value = GetValue(key);
            
            var data = Convert.FromBase64String(value);
            var encoding = Encoding.Unicode;
            var decryptedBytes = ProtectedData.Unprotect(data, encoding.GetBytes(Entropy), DataProtectionScope.LocalMachine);
            
            var secureString = new SecureString();
            foreach (var @char in encoding.GetChars(decryptedBytes))
            {
              secureString.AppendChar(@char);  
            }

            return secureString;
        }

        public void SetSecureValue(string key, string value)
        {
            var encoding = Encoding.Unicode;
            var data = encoding.GetBytes(value);
            var encryptedBytes = ProtectedData.Protect(data, encoding.GetBytes(Entropy), DataProtectionScope.LocalMachine);
            var encryptedValue = Convert.ToBase64String(encryptedBytes);

            SetValue(key, encryptedValue);
        }

        public void Remove(string key)
        {
            _config.AppSettings.Settings.Remove(key);
        }

        public void Remove(params string[] keys)
        {
            var settings = _config.AppSettings.Settings;
            
            foreach (var key in keys)
                settings.Remove(key);
        }

        public void Save()
        {
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }






        #region Internal
        private void AddElement(string key, string value)
        {
            var element = new KeyValueConfigurationElement(key, value);
            SetElement(element);
        }

        private void SetElement(KeyValueConfigurationElement element)
        {
            _config.AppSettings.Settings.Add(element);
        }

        private KeyValueConfigurationElement GetElement(string key)
        {
            var element = _config.AppSettings.Settings[key];
            return element;
        }

        ~ConfigurationProvider()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                _config.Save(ConfigurationSaveMode.Modified);
            
            _disposed = true;
        }
        #endregion Internal
    }
}