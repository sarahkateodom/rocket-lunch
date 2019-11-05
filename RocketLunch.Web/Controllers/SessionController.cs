using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using Microsoft.AspNetCore.Mvc;

namespace RocketLunch.web.controllers
{
    public class SessionController : Controller
    {
        IManageUserSessions _sessionService;
        public SessionController(IManageUserSessions sessionService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException("sessionService");
        }

        [HttpPost]
        [Route("api/sessions")]
        public Guid CreateSession([FromBody] List<int> userIds)
        {
            return _sessionService.CreateUserSession(userIds);
        }

        [HttpPut]
        [Route("api/sessions/{sessionId}")]
        public bool UpdateSession(Guid sessionId, [FromBody] List<int> userIds)
        {
            _sessionService.UpdateUserSession(sessionId, userIds);
            return true;
        }
    }
}