using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using DataLayer.Configurations;
using System.Linq;

namespace DataLayer.DbContext
{
    public class StoryDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Story entity
            modelBuilder.Entity<Story>()
                .HasMany(s => s.Sentences)
                .WithOne()
                .HasForeignKey("StoryId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Story>()
                .HasMany(s => s.Units)
                .WithOne()
                .HasForeignKey("StoryId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Sentence entity with composite key
            modelBuilder.Entity<Sentence>()
                .HasKey(s => new { s.Id, s.StoryId });

            // Configure Unit entity with composite key
            modelBuilder.Entity<Unit>()
                .HasKey(u => new { u.Id, u.StoryId });

            // Configure Unit.Segments as owned entity type
            modelBuilder.Entity<Unit>()
                .OwnsMany(u => u.Segments);

            // Configure Unit.Pieces as a converted collection
            modelBuilder.Entity<Unit>()
                .Property(u => u.Pieces)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            // Configure UserPreferences.SeenStoryIds as a converted collection
            modelBuilder.Entity<UserPreferences>()
                .Property(u => u.SeenStoryIds)
                .HasConversion(
                    v => string.Join(',', v.Select(id => id.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(id => Guid.Parse(id))
                         .ToList()
                );

            // Configure UserPreferences.SavedStoryIds as a converted collection
            modelBuilder.Entity<UserPreferences>()
                .Property(u => u.SavedStoryIds)
                .HasConversion(
                    v => string.Join(',', v.Select(id => id.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(id => Guid.Parse(id))
                         .ToList()
                );
        }
    }
}