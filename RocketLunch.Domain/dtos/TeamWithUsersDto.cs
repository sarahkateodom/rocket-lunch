using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class TeamWithUsersDto : TeamDto
    {
        public TeamWithUsersDto()
        {
            this.Users = new List<UserDto>();
        }
        public IEnumerable<UserDto> Users { get; set; }
    }
}