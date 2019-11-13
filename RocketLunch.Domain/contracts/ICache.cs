using System;
using System.Threading.Tasks;

namespace RocketLunch.domain.contracts
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expirationSpan = null);
    }
}