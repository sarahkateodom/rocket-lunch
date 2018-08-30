using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace makelunch.data.entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nopes { get; set; }
    }
}