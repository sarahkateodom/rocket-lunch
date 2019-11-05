using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RocketLunch.data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName()); 

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(ToSnakeCase(key.GetName()));
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(ToSnakeCase(key.GetConstraintName()));
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(ToSnakeCase(index.GetName()));
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
