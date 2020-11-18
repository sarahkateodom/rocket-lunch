using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RocketLunch.data.entities;
using Microsoft.EntityFrameworkCore;
using RocketLunch.domain.utilities;

namespace RocketLunch.data
{
    public class LunchContext : DbContext
    {
        public LunchContext() : this(new DbContextOptionsBuilder<LunchContext>().UseMySQL("Server=localhost;Database=rocketlunch;Uid=admin;Pwd=admin;Port=3306").Options)
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
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TeamEntity>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

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
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }
        }


    }
}
