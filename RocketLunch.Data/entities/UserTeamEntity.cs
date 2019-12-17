namespace RocketLunch.data.entities
{
    public class UserTeamEntity
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }

        public UserEntity User { get; set; }
        public TeamEntity Team { get; set; }
    }
}