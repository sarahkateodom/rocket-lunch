using System;
using System.Collections.Generic;
using System.Linq;
using makelunch.domain.contracts;
using makeLunch.domain.utilities;

namespace makelunch.domain.services
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