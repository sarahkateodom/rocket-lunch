using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.exceptions;
using RocketLunch.domain.utilities;
using Newtonsoft.Json;
using System.Security.Claims;

namespace RocketLunch.domain.services
{
    public class UserService : IManageUsers
    {
        private IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
        }

        public async Task<ClaimsPrincipal> LoginAsync(LoginDto userDto)
        {
            // if user is new, create User record
            UserDto user = await _repository.GetUserAsync(userDto.GoogleId)
                ?? await _repository.CreateUserAsync(userDto.GoogleId, userDto.Email, userDto.Name);

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));

            return new ClaimsPrincipal(identity);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, int>> CreateUserAsync(CreateUserDto dto)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (dto == null) throw new ValidationException("CreateUserDto is required");
                if (String.IsNullOrWhiteSpace(dto.Name)) throw new ValidationException("User name is required");

                return await _repository.CreateUserAsync_Old(dto.Name, dto.Nopes).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>> GetUsersAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                return await _repository.GetUsersAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, bool>> UpdateUserAsync(UserDto dto)
        {
            return await ExceptionHandler.HandleExceptionAsync((Func<Task<bool>>)(async () =>
            {
                UserDto user = await _repository.GetUserAsync((int)dto.Id).ConfigureAwait(false) ?? throw new NotFoundException("Specified user not found.");

                await _repository.UpdateUserAsync(dto.Id, dto.Name, dto.Nopes).ConfigureAwait(false);
                return true;
            })).ConfigureAwait(false);
        }
    }
}