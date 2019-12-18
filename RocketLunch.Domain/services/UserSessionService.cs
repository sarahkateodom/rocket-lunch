using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.services
{
    public class UserSessionService : IManageUserSessions
    {
        IRestaurantCache cache;
        public UserSessionService(IRestaurantCache cache)
        {
            this.cache = cache;
        }

        public async Task<Guid> CreateUserSession(IEnumerable<int> userIds)
        {
            Guid sessionGuid = Guid.NewGuid();
            await cache.SetUserSessionAsync(sessionGuid, userIds.ToList());
            return sessionGuid;
        }

        //  public async Task UpdateUserSession(Guid sessionId, IEnumerable<int> userIds)
        // {
        //     await cache.SetUserSessionAsync(sessionId, userIds.ToList());
        // }        
    }
}