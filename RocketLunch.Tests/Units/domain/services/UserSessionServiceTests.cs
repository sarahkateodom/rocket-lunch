using System;
using System.Collections.Generic;
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;
using RocketLunch.domain.utilities;
using Xunit;

namespace RocketLunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class UserSessionServiceTests
    {
        [Fact]
        public async void SessionService_CreateUserSession_AddsUserIdsToCash()
        {
            //arrange
            List<int> userIds = new List<int> {
                1,2,3 // ha ha ha
            };
            Mock<IRestaurantCache> mockCache = new Mock<IRestaurantCache>();
            UserSessionService target = new UserSessionService(mockCache.Object);

            //act
            Guid result = await target.CreateUserSession(userIds);

            //assert
            mockCache.Verify(x => x.SetUserSessionAsync(result, userIds), Times.Once);

        }

        [Fact]
        public async void SessionService_UpdateUserSession_UpdatesUserIdsInCash()
        {
            //arrange
            List<int> userIds = new List<int> {
                1,2,3 // ha ha ha
            };
            Mock<IRestaurantCache> mockCache = new Mock<IRestaurantCache>();
            Guid sessionGuid = Guid.NewGuid();
            mockCache.Setup(x => x.GetUserSessionAsync(sessionGuid)).ReturnsAsync(userIds);
            List<int> newUserIds = new List<int> {
                7,8,9 // ha ha ha
            };
            UserSessionService target = new UserSessionService(mockCache.Object);

            //act
            await target.UpdateUserSession(sessionGuid, newUserIds);

            //assert
            mockCache.Verify(x => x.SetUserSessionAsync(sessionGuid, newUserIds), Times.Once);

        }
    }
}