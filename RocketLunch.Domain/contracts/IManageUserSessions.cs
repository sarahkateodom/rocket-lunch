using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RocketLunch.domain.contracts
{
    public interface IManageUserSessions
    {
        Task<Guid> CreateUserSession(IEnumerable<int> userIds);
        // Task UpdateUserSession(Guid sessionId, IEnumerable<int> userIds);
    }
}