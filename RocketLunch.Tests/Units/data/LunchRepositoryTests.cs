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
                Zip = "90210"
            }).Entity;

            context.SaveChanges();

            // act
            UserDto result = await target.GetUserAsync(addedUserSettings.GoogleId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(JsonConvert.SerializeObject(nopes), JsonConvert.SerializeObject(result.Nopes));
        }

        [Fact]
        public async void LunchRepository_GetUserAsync_GetsUserWithTeams()
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
                Zip = "90210"
            }).Entity;

            TeamEntity team1 = context.Teams.Add(new TeamEntity
            {
                Id = 1,
                Name = "bob's Team",
                Zip = "38655"
            }).Entity;
            TeamEntity team2 = context.Teams.Add(new TeamEntity
            {
                Id = 2,
                Name = "lilTimmy's Team",
                Zip = "38655"
            }).Entity;
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = 1,
                TeamId = team1.Id
            });
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = 1,
                TeamId = team2.Id
            });

            context.SaveChanges();

            // act
            UserWithTeamsDto result = await target.GetUserAsync(addedUserSettings.GoogleId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Teams.Count());
            Assert.Equal(team1.Name, result.Teams.ElementAt(0).Name);
            Assert.Equal(team2.Name, result.Teams.ElementAt(1).Name);
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
        public async void LunchRepository_GetUserByEmailAsync_GetsUser()
        {
            // arrangec
            LunchContext context = GetContext();
            List<string> nopes = new List<string> { "https://goo.gl/pUu7he" };
            var addedUser = context.Users.Add(new UserEntity
            {
                Id = 1,
                GoogleId = "googleID",
                Name = "test",
                Email = "email@email.email",
                Nopes = JsonConvert.SerializeObject(nopes),
                PhotoUrl = "https://gph.is/NYMue5",
                Zip = "39955",
            }).Entity;

            context.SaveChanges();

            LunchRepository target = new LunchRepository(context);

            // act
            UserDto result = await target.GetUserByEmailAsync(addedUser.Email);

            // assert
            Assert.Equal(addedUser.Email, result.Email);
            Assert.Equal(addedUser.Id, result.Id);
            Assert.Equal(addedUser.Name, result.Name);
            Assert.Equal(addedUser.Nopes, JsonConvert.SerializeObject(result.Nopes));
        }


        [Fact]
        public async void LunchRepository_GetUserByEmailAsync_ReturnsNullWhenUserNotFound()
        {
            // arrangec
            LunchContext context = GetContext();

            LunchRepository target = new LunchRepository(context);

            // act
            UserDto result = await target.GetUserByEmailAsync("fsdfklasjf");

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
            await target.UpdateUserAsync(user.Id, name, nopes, "90210");

            // assert
            UserEntity updatedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.Equal(name, updatedUser.Name);
            Assert.Equal(JsonConvert.SerializeObject(nopes), updatedUser.Nopes);
            Assert.Equal("90210", updatedUser.Zip);
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
        public async void LunchRepository_GetUserAsync_InternalId_GetsUserWithTeams()
        {
            // arrangec
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            List<string> nopes = new List<string> { "https://goo.gl/pUu7he" };
            var addedUser = context.Users.Add(new UserEntity
            {
                Id = 1,
                GoogleId = "googleID",
                Name = "test",
                Nopes = JsonConvert.SerializeObject(nopes),
                Zip = "90210"
            }).Entity;

            TeamEntity team1 = context.Teams.Add(new TeamEntity
            {
                Id = 1,
                Name = "bob's Team",
                Zip = "38655"
            }).Entity;
            TeamEntity team2 = context.Teams.Add(new TeamEntity
            {
                Id = 2,
                Name = "lilTimmy's Team",
                Zip = "38655"
            }).Entity;
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = 1,
                TeamId = team1.Id
            });
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = 1,
                TeamId = team2.Id
            });

            context.SaveChanges();

            // act
            UserWithTeamsDto result = await target.GetUserAsync(addedUser.Id);

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Teams.Count());
            Assert.Equal(team1.Name, result.Teams.ElementAt(0).Name);
            Assert.Equal(team2.Name, result.Teams.ElementAt(1).Name);
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

        [Fact]
        public async void LunchRepository_CreateTeamAsync_CreatesATeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            const string Name = "bob's team";
            const string Zip = "90210";

            // act
            int result = await target.CreateTeamAsync(Name, Zip);

            // assert
            var team = context.Teams.First();
            Assert.Equal(Name, team.Name);
            Assert.Equal(Zip, team.Zip);
            Assert.Equal(team.Id, result);
        }

        [Fact]
        public async void LunchRepository_GetTeamAsync_ReturnsSpecifiedTeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            TeamEntity team = context.Teams.Add(new TeamEntity()
            {
                Name = "Tahra Dactyls",
                Zip = "90210",
            }).Entity;
            await context.SaveChangesAsync();

            // act
            TeamDto result = await target.GetTeamAsync(team.Id);

            // assert
            Assert.Equal(team.Name, result.Name);
            Assert.Equal(team.Zip, result.Zip);
        }

        [Fact]
        public async void LunchRepository_GetTeamAsync_ReturnsNullIfTeamDoesNotExist()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            // act
            TeamDto result = await target.GetTeamAsync(8888);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async void LunchRepository_UpdateTeamAsync_UpdatesTeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            const string Name = "bob's team";
            const string Zip = "90210";
            const string NewName = "fanny's team";
            const string NewZip = "00501";

            int id = await target.CreateTeamAsync(Name, Zip);
            await target.UpdateTeamAsync(id, NewName, NewZip);
            
            // act

            var result = context.Teams.Where( u => u.Id == id).Single();
        
            // assert
            Assert.Equal(NewName, result.Name);
            Assert.Equal(NewZip, result.Zip);

        }
        
        [Fact]
        public async void LunchRepository_TeamNameExistsAsync_ReturnsFalseWhenTeamDoesNotExist()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            const string Name = "bob's team";

            // act
            bool result = await target.TeamNameExistsAsync(Name);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async void LunchRepository_TeamNameExistsAsync_ReturnsTrueWhenTeamDoesExist()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            const string Name = "bob's team";
            context.Teams.Add(new TeamEntity
            {
                Name = Name
            });
            context.SaveChanges();

            // act
            bool result = await target.TeamNameExistsAsync(Name);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async void LunchRepository_AddUserToTeamAsync_AddsUserToTeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            TeamEntity team = context.Teams.Add(new TeamEntity
            {
                Name = "bob's team"
            }).Entity;
            UserEntity user = context.Users.Add(new UserEntity
            {
                Name = "bob"
            }).Entity;
            context.SaveChanges();


            // act
            await target.AddUserToTeamAsync(user.Id, team.Id);

            // assert
            var userTeam = context.UserTeams.First();
            Assert.Equal(team.Id, userTeam.TeamId);
            Assert.Equal(user.Id, userTeam.UserId);
        }

        [Fact]
        public async void LunchRepository_AddUserToTeamAsync_ReturnsWithoutErrorWhenComboAlreadyExists()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            TeamEntity team = context.Teams.Add(new TeamEntity
            {
                Name = "bob's team"
            }).Entity;
            UserEntity user = context.Users.Add(new UserEntity
            {
                Name = "bob"
            }).Entity;
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = user.Id,
                TeamId = team.Id
            });
            context.SaveChanges();


            // act
            await target.AddUserToTeamAsync(user.Id, team.Id);

            // assert
            // no exception thrown
        }

        [Fact]
        public async void LunchRepository_RemoveUserFromTeamAsync_RemovesUserFromTeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            TeamEntity team = context.Teams.Add(new TeamEntity
            {
                Name = "bob's team"
            }).Entity;
            UserEntity user = context.Users.Add(new UserEntity
            {
                Name = "bob"
            }).Entity;
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = user.Id,
                TeamId = team.Id
            });
            context.SaveChanges();


            // act
            await target.RemoveUserFromTeamAsync(user.Id, team.Id);

            // assert
            Assert.Equal(0, context.UserTeams.Count());
        }

        [Fact]
        public async void LunchRepository_GetUsersOfTeamAsync_ReturnsListOfUsersFromTeam()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            TeamEntity team = context.Teams.Add(new TeamEntity
            {
                Name = "bob's team"
            }).Entity;
            UserEntity user = context.Users.Add(new UserEntity
            {
                Name = "bob"
            }).Entity;
            UserEntity user2 = context.Users.Add(new UserEntity
            {
                Name = "lilTimmy"
            }).Entity;
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = user.Id,
                TeamId = team.Id
            });
            context.UserTeams.Add(new UserTeamEntity
            {
                UserId = user2.Id,
                TeamId = team.Id
            });
            context.SaveChanges();


            // act
            List<UserDto> result = (await target.GetUsersOfTeamAsync(team.Id)).ToList();

            // assert
            Assert.Equal(2, result.Count());
            Assert.Equal("bob", result[0].Name);
            Assert.Equal("lilTimmy", result[1].Name);
        }

        [Fact]
        public async void LunchRepository_GetUsersOfTeamAsync_ReturnsNullWhenTeamNotFound()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);

            // act
            IEnumerable<UserDto> result = await target.GetUsersOfTeamAsync(1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async void LunchRepository_GetNopesAsync_ReturnsUsersNopes()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            UserEntity user = context.Users.Add(new UserEntity
            {
                Name = "bob",
                Nopes = "['thing1','thing2']",
            }).Entity;
            UserEntity user2 = context.Users.Add(new UserEntity
            {
                Name = "lilTimmy",
                Nopes = "['thing3','thing1']",
            }).Entity;
            context.SaveChanges();

            IEnumerable<int> ids = context.Users.Select(u => u.Id);

            // act
            IEnumerable<string> result = await target.GetNopesAsync(ids);

            // assert
            Assert.Contains("thing1", result);
            Assert.Contains("thing2", result);
            Assert.Contains("thing3", result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async void LunchRepository_GetNopesAsync_ReturnsEmptyListWithNoUsers()
        {
            // arrange
            LunchContext context = GetContext();
            LunchRepository target = new LunchRepository(context);
            context.SaveChanges();

            var ids = new List<int>();

            // act
            IEnumerable<string> result = await target.GetNopesAsync(ids);

            // assert
            Assert.Equal(0, result.Count());
        }


        private LunchContext GetContext()
        {
            // NOTE: README: THIS HAPPENED:  Keys in DB do not automatically reset with new in memory db, see:  https://github.com/aspnet/EntityFrameworkCore/issues/6872
            return new LunchContext(new DbContextOptionsBuilder<LunchContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }
    }
}