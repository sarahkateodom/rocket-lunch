using System;
using System.Collections.Generic;
using System.Linq;
using makelunch.data;
using makelunch.data.entities;
using makelunch.domain.dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace makelunch.tests.units.data
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
        public async void LunchRepository_CreateUserAsyncr_AddsEntryToTable()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            string name = "Tahra Dactyl";
            List<string> nopes = new List<string> { "https://goo.gl/pUu7he" };

            // act
            await target.CreateUserAsync(name, nopes);

            // assert
            UserEntity newUser = context.Users.Where(u => u.Name == name).FirstOrDefault();
            Assert.NotNull(newUser);
            Assert.True(newUser.Id > 0);
            Assert.Equal(JsonConvert.SerializeObject(nopes), newUser.Nopes);
        }

        [Fact]
        public async void LunchRepository_CreateUserAsync_ReturnsId()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            string name = "Tahra Dactyl";
            List<string> nopes = new List<string> { "https://goo.gl/pUu7he" };

            // act
            int result = await target.CreateUserAsync(name, nopes);

            // assert



            UserEntity newUser = context.Users.Where(u => u.Name == name).FirstOrDefault();
            Assert.Equal(newUser.Id, result);
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