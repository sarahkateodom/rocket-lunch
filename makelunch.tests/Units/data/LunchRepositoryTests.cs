using System;
using System.Collections.Generic;
using System.Linq;
using makelunch.data;
using makelunch.data.entities;
using makelunch.domain.dtos;
using Microsoft.EntityFrameworkCore;
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
            string nopes = "https://goo.gl/pUu7he";                          

            // act
            await target.CreateUserAsync(name, nopes);

            // assert
            UserEntity newUser = context.Users.Where(u => u.Name == name).FirstOrDefault();
            Assert.NotNull(newUser);
            Assert.True(newUser.Id > 0);
            Assert.Equal(nopes, newUser.Nopes);
        }

        [Fact]
        public async void LunchRepository_CreateUserAsync_ReturnsId()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            string name = "Tahra Dactyl";
            string nopes = "https://goo.gl/pUu7he";

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
            context.Users.Add(new UserEntity() {
                Name = "Tahra Dactyl"
            });

            context.SaveChanges();

            // act
            List<UserDto> result = (await target.GetUsersAsync()).ToList();

            // assert
            Assert.Equal(context.Users.Count(), result.Count);
        }


        private LunchContext GetContext()
        {
            // NOTE: README: THIS HAPPENED:  Keys in DB do not automatically reset with new in memory db, see:  https://github.com/aspnet/EntityFrameworkCore/issues/6872
            return new LunchContext(new DbContextOptionsBuilder<LunchContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }
    }
}