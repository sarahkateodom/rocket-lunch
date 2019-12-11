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

        public async Task<UserDto> LoginAsync(LoginDto userDto)
        {
            // if user is new, create User record
            return await _repository.GetUserAsync(userDto.GoogleId)
                ?? await _repository.CreateUserAsync(userDto.GoogleId, userDto.Email, userDto.Name, userDto.PhotoUrl);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return await _repository.GetUsersAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto)
        {
            UserDto user = await _repository.GetUserAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException("Specified user not found.");

            await _repository.UpdateUserAsync(userId, dto.Name, dto.Nopes).ConfigureAwait(false);
            return true;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            UserDto user = await _repository.GetUserAsync(id);
            if (user == null) throw new NotFoundException();
            return user;
        }
    }
}