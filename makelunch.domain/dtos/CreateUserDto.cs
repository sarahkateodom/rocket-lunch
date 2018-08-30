using System.Collections.Generic;

namespace makelunch.domain.dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public List<string> Nopes { get; set; }
    }
}