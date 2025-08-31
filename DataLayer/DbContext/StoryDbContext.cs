using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using DataLayer.Configurations;

namespace DataLayer.DbContext
{
    public class StoryDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Story> Stories { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new StoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserPreferencesConfiguration());
        }
    }
}