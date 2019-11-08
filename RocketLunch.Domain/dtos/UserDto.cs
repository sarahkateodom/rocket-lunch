using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public List<string> Nopes { get; set; }
    }
}