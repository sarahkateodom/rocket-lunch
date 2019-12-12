using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RocketLunch.data.entities;
using Microsoft.EntityFrameworkCore;

namespace RocketLunch.data
{
    public class LunchContext : DbContext
    {
        public LunchContext() : this(new DbContextOptionsBuilder<LunchContext>().UseMySQL("Uid=admin;Pwd=admin;Server=localhost;Port=5433;Database=rocketlunch").Options)
        {

        }

        public LunchContext(DbContextOptions<LunchContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }

        public string ProviderName => base.Database.ProviderName;

        public void Migrate()
        {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<UserEntity>()
            //     .Property(r => r.Nopes)
            //     .HasDefaultValue("[]");

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
