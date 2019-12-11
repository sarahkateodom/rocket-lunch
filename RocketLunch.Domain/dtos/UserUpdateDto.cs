using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public List<string> Nopes { get; set; }
        public string Zip { get; set; }

    }
}