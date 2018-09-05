using System;
using System.Collections.Generic;

namespace makelunch.domain.contracts
{
    public interface IManageUserSessions
    {
        Guid CreateUserSession(IEnumerable<int> userIds);
        void UpdateUserSession(Guid sessionId, IEnumerable<int> userIds);
    }
}