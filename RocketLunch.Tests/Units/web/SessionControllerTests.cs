using System;
using System.Collections.Generic;
using RocketLunch.domain.contracts;
using RocketLunch.web.controllers;
using Moq;
using Xunit;

namespace RocketLunch.tests.units.web
{

    [Trait("Category", "Unit")]
    public class SessionControllerTests 
    {
        // [Fact]
        // public void SessionController_Ctor_RequiresSessionService()
        // {
        //     // Act & Assert
        //     Assert.Throws<ArgumentNullException>(() => new SessionController((IManageUserSessions)null));
        // }

        // [Fact]
        // public async void SessionController_CreateSession_CallsUserSessionService()
        // {
        //     //arrange
        //     Mock<IManageUserSessions> mockSessions = new Mock<IManageUserSessions>();
        //     mockSessions.Setup(x => x.CreateUserSession(It.IsAny<IEnumerable<int>>())).ReturnsAsync(Guid.NewGuid());
        //     SessionController target = new SessionController(mockSessions.Object);
        //     List<int> users = new List<int> {
        //         1, 2, 3
        //     };

        //     // Act
        //     Guid result = await target.CreateSession(users);

        //     // Assert
        //     mockSessions.Verify(x => x.CreateUserSession(users), Times.Once);
        // }

        // [Fact]
        // public async void SessionController_UpdateSession_CallsUserSessionService()
        // {
        //     //arrange
        //     Mock<IManageUserSessions> mockSessions = new Mock<IManageUserSessions>();
        //     SessionController target = new SessionController(mockSessions.Object);
        //     Guid sessionGuid = Guid.NewGuid();
        //     List<int> users = new List<int> {
        //         1, 2, 3
        //     };

        //     // Act
        //     await target.UpdateSession(sessionGuid, users);

        //     // Assert
        //     mockSessions.Verify(x => x.UpdateUserSession(sessionGuid, users), Times.Once);
        // }
    }
}