using System;
using System.Collections.Generic;
using System.Linq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.services
{
    public class UserSessionService : IManageUserSessions
    {
        public Guid CreateUserSession(IEnumerable<int> userIds)
        {
            Guid sessionGuid = Guid.NewGuid();
            RestaurantCash.CreateUpdateUserSession(sessionGuid, userIds.ToList());
            return sessionGuid;
        }

         public void UpdateUserSession(Guid sessionId, IEnumerable<int> userIds)
        {
            RestaurantCash.CreateUpdateUserSession(sessionId, userIds.ToList());
        }        
    }
}