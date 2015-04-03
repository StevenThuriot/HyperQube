using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace HyperQube.Library
{
    public interface IConfigurationProvider
    {
        string GetAPIKey();
        void SetAPIKey(string value);

        string GetValue(string key);
        IDictionary<string, string> GetValues(params string[] keys);
        void SetValue(string key, string value);
        void SetValues(IDictionary<string, string> kvps);
        SecureString GetSecureValue(string key);
        void SetSecureValue(string key, string value);
        void Remove(string key);
        void Remove(params string[] keys);
        void Save();

        Task<string> GetValueAsync(string key);
        Task<IDictionary<string, string>> GetValuesAsync(params string[] keys);
        Task SetValueAsync(string key, string value);
        Task SetValuesAsync(IDictionary<string, string> kvps);
        Task<SecureString> GetSecureValueAsync(string key);
        Task SetSecureValueAsync(string key, string value);
        Task SaveAsync();

        Task<string> GetAPIKeyAsync();
        Task SetAPIKeyAsync(string value);
        Task RemoveAsync(string key);
        Task RemoveAsync(params string[] keys);
    }
}