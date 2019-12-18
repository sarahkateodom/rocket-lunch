using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class UserWithTeamsDto: UserDto
    {
        public UserWithTeamsDto()
        {
            this.Teams = new List<TeamDto>();
        }
        public IEnumerable<TeamDto> Teams { get; set; }
    }
}