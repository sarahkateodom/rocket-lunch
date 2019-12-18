using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RocketLunch.data.entities;
using Microsoft.EntityFrameworkCore;

namespace RocketLunch.data
{
    public class LunchContext : DbContext
    {
        public LunchContext() : this(new DbContextOptionsBuilder<LunchContext>().UseNpgsql("User ID=admin;Password=admin;Host=localhost;Port=5433;Database=rocketlunch").Options)
        {

        }

        public LunchContext(DbContextOptions<LunchContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<UserTeamEntity> UserTeams { get; set; }

        public string ProviderName => base.Database.ProviderName;

        public void Migrate()
        {
            this.Database.Migrate();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .Property(r => r.Nopes)
                .HasDefaultValue("[]");

            modelBuilder.Entity<UserEntity>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseNpgsqlIdentityByDefaultColumn();

            modelBuilder.Entity<TeamEntity>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseNpgsqlIdentityByDefaultColumn();

            modelBuilder.Entity<UserTeamEntity>()
                .HasKey(p => new { p.TeamId, p.UserId });
            modelBuilder.Entity<UserTeamEntity>()
                .HasOne(bc => bc.Team)
                .WithMany(b => b.TeamUsers)
                .HasForeignKey(bc => bc.TeamId);
            modelBuilder.Entity<UserTeamEntity>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.UserTeams)
                .HasForeignKey(bc => bc.UserId);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = ToSnakeCase(entity.Relational().TableName);

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = ToSnakeCase(property.Name);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = ToSnakeCase(index.Relational().Name);
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
