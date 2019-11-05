using System;
using System.Collections.Generic;
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
        public void SessionService_CreateUserSession_AddsUserIdsToCash()
        {
            //arrange
            List<int> userIds = new List<int> {
                1,2,3 // ha ha ha
            };
            UserSessionService target = new UserSessionService();

            //act
            Guid result = target.CreateUserSession(userIds);

            //assert
            List<int> returnedUsers = RestaurantCash.GetUserSession(result.ToString());
            Assert.Equal(userIds[0], returnedUsers[0]);
            Assert.Equal(userIds[1], returnedUsers[1]);
            Assert.Equal(userIds[2], returnedUsers[2]);

        }

        [Fact]
        public void SessionService_UpdateUserSession_UpdatesUserIdsInCash()
        {
            //arrange
            List<int> userIds = new List<int> {
                1,2,3 // ha ha ha
            };
            Guid sessionGuid = Guid.NewGuid();
            RestaurantCash.CreateUpdateUserSession(sessionGuid, userIds);
            List<int> newUserIds = new List<int> {
                7,8,9 // ha ha ha
            };
            UserSessionService target = new UserSessionService();

            //act
            target.UpdateUserSession(sessionGuid, newUserIds);

            //assert
            List<int> returnedUsers = RestaurantCash.GetUserSession(sessionGuid.ToString());
            Assert.Equal(newUserIds[0], returnedUsers[0]);
            Assert.Equal(newUserIds[1], returnedUsers[1]);
            Assert.Equal(newUserIds[2], returnedUsers[2]);

        }
    }
}