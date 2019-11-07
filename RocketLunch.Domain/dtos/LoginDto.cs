using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class LoginDto
    {
        public string GoogleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}