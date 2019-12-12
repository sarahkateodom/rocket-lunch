using System;
using System.Collections.Generic;
using System.Linq;
using RocketLunch.data;
using RocketLunch.data.entities;
using RocketLunch.domain.dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace RocketLunch.tests.units.data
{
    [Trait("Category", "Unit")]
    public class LunchRepositoryTests
    {
        [Fact]
        public void LunchRepository_Ctor_RequiresContext()
        {
            Assert.Throws<ArgumentNullException>(() => new LunchRepository((LunchContext)null));
        }

        [Fact]
        public void LunchRepository_Ctor_ReturnsRepository()
        {
            // arrange
            LunchContext context = GetContext();

            // act
            LunchRepository repo = new LunchRepository(context);

            // assert
            Assert.NotNull(repo);
        }

        [Fact]
        public async void LunchRepository_GetUserAsync_GetsUser()
        {
            // arrangec
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            List<string> nopes = new List<string> { "https://goo.gl/pUu7he" };
            var addedUserSettings = context.Users.Add(new UserEntity
            {
                Id = 1,
                GoogleId = "googleID",
                Name = "test",
                Nopes = JsonConvert.SerializeObject(nopes),
            }).Entity;

            context.SaveChanges();

            // act
            UserDto result = await target.GetUserAsync(addedUserSettings.GoogleId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(JsonConvert.SerializeObject(nopes), JsonConvert.SerializeObject(result.Nopes));
        }

        [Fact]
        public async void LunchRepository_GetUserAsync_ReturnsNullWhenUserNotFound()
        {
            // arrangec
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            // act
            UserDto result = await target.GetUserAsync("GoogleId");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async void LunchRepository_CreateUserAsync_AddsEntryToTable()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            string googleId = "googleId";
            string email = "email@e.mail";
            string name = "goodname";
            string photoUrl = "photo";

            // act
            await target.CreateUserAsync(googleId, email, name, photoUrl);

            // assert
            UserEntity newUser = context.Users.Where(u => u.GoogleId == googleId).FirstOrDefault();
            Assert.True(newUser.Id > 0);
            Assert.Equal(email, newUser.Email);
            Assert.Equal(name, newUser.Name);
        }

        [Fact]
        public async void LunchRepository_CreateUserAsync_ReturnsId()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            string googleId = "googleId";
            string email = "email@e.mail";
            string name = "goodname";
            string photoUrl = "photo";

            // act
            var result = await target.CreateUserAsync(googleId, email, name, photoUrl);

            // assert
            UserEntity newUser = context.Users.Where(u => u.Name == name).FirstOrDefault();
            Assert.Equal(newUser.Id, result.Id);
        }

        [Fact]
        public async void LunchRepository_GetUsersAsync_ReturnsUsers()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            context.Users.Add(new UserEntity()
            {
                Id = 1,
                Name = "Tahra Dactyl",
                Nopes = "[]",
            });

            context.SaveChanges();

            // act
            List<UserDto> result = (await target.GetUsersAsync()).ToList();


            // assert
            Assert.Equal(context.Users.Count(), result.Count);
        }

        [Fact]
        public async void LunchRepository_UpdateUserAsync_UpdatesUser()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            UserEntity user = context.Users.Add(new UserEntity()
            {
                Name = "Tahra Dactyl",
                Nopes = "[]",
            }).Entity;
            await context.SaveChangesAsync();

            string name = "Paul R. Baer";
            List<string> nopes = new List<string> { "Chum Bucket", "Jimmy Pesto's Pizzaria" };

            // act
            await target.UpdateUserAsync(user.Id, name, nopes);

            // assert
            UserEntity updatedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.Equal(name, updatedUser.Name);
            Assert.Equal(JsonConvert.SerializeObject(nopes), updatedUser.Nopes);
        }

        [Fact]
        public async void LunchRepository_GetUserAsync_ReturnsSpecifiedUser()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            UserEntity user = context.Users.Add(new UserEntity()
            {
                Name = "Tahra Dactyl",
                Nopes = "[]",
            }).Entity;
            await context.SaveChangesAsync();

            // act
            UserDto result = await target.GetUserAsync(user.Id);

            // assert
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Nopes, JsonConvert.SerializeObject(result.Nopes));
        }

        [Fact]
        public async void LunchRepository_GetUserAsync_ReturnsNullIfUserDoesNotExist()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            // act
            UserDto result = await target.GetUserAsync(8888);

            // assert
            Assert.Null(result);
        }


        private LunchContext GetContext()
        {
            // NOTE: README: THIS HAPPENED:  Keys in DB do not automatically reset with new in memory db, see:  https://github.com/aspnet/EntityFrameworkCore/issues/6872
            return new LunchContext(new DbContextOptionsBuilder<LunchContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }
    }
}