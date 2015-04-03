using System;

namespace HyperQube.Library
{
    public interface IConnectionService
    {
        IDisposable OpenConnection(string apiKey);
    }
}