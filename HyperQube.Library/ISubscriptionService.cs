using System;

namespace HyperQube.Library
{
    public interface ISubscriptionService : IDisposable
    {
        void EnableOrDisableQubes();
    }
}
