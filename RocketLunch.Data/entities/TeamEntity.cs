using System.ComponentModel.DataAnnotations;

namespace RocketLunch.data.entities
{
    public class TeamEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Zip { get; set; }
    }
}