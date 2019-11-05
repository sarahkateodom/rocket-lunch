using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public List<string> Nopes { get; set; }
    }
}