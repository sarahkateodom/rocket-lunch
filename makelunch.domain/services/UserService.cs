using System;
using System.Threading.Tasks;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.domain.Exceptions;
using makeLunch.domain.utilities;

namespace makelunch.domain.services
{
    public class UserService : IManageUsers
    {
        private IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, bool>> CreateUserAsync(CreateUserDto dto)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                if(dto == null) throw new ValidationException("CreateUserDto is required");
                if(String.IsNullOrWhiteSpace(dto.Name)) throw new ValidationException("User name is required");

                await _repository.CreateUserAsync(dto.Name, dto.AvatarUrl).ConfigureAwait(false);
                return true;
            }).ConfigureAwait(false);
        }
    }
}