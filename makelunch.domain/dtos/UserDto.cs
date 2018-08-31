using System.Collections.Generic;

namespace makelunch.domain.dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Nopes { get; set; }
    }
}