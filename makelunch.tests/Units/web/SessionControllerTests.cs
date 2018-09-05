using System;
using System.Collections.Generic;
using makelunch.domain.contracts;
using makelunch.web.controllers;
using Moq;
using Xunit;

namespace makelunch.tests.units.web
{

    [Trait("Category", "Unit")]
    public class SessionControllerTests 
    {
        [Fact]
        public void SessionController_Ctor_RequiresSessionService()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SessionController((IManageUserSessions)null));
        }

        [Fact]
        public void SessionController_CreateSession_CallsUserSessionService()
        {
            //arrange
            Mock<IManageUserSessions> mockSessions = new Mock<IManageUserSessions>();
            mockSessions.Setup(x => x.CreateUserSession(It.IsAny<IEnumerable<int>>())).Returns(Guid.NewGuid());
            SessionController target = new SessionController(mockSessions.Object);
            List<int> users = new List<int> {
                1, 2, 3
            };

            // Act
            Guid result = target.CreateSession(users);

            // Assert
            mockSessions.Verify(x => x.CreateUserSession(users), Times.Once);
        }

        [Fact]
        public void SessionController_UpdateSession_CallsUserSessionService()
        {
            //arrange
            Mock<IManageUserSessions> mockSessions = new Mock<IManageUserSessions>();
            SessionController target = new SessionController(mockSessions.Object);
            Guid sessionGuid = Guid.NewGuid();
            List<int> users = new List<int> {
                1, 2, 3
            };

            // Act
            target.UpdateSession(sessionGuid, users);

            // Assert
            mockSessions.Verify(x => x.UpdateUserSession(sessionGuid, users), Times.Once);
        }
    }
}