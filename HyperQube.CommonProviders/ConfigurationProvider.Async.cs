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

        public Task<IDictionary<string, string>> GetValuesAsync(params string[] keys)
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