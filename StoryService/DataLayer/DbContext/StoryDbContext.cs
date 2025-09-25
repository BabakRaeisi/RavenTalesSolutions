using System;
using System.Collections.Generic;
using System.Linq;
using CoreLayer.Entities;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataLayer.DbContext
{
    using Microsoft.EntityFrameworkCore;
    public class StoryDbContext : DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options) { }

        public DbSet<Story> Stories { get; set; } = null!;
        public DbSet<Sentence> Sentences { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------- Story (no UserId, no IsBookmarked) --------
            modelBuilder.Entity<Story>(e =>
            {
                e.HasKey(s => s.Id);

                e.Property(s => s.Title).IsRequired().HasMaxLength(255).IsUnicode(true);
                e.Property(s => s.Genre).HasMaxLength(100).IsUnicode(true);

                e.Property(s => s.Content)
                 .IsRequired()
                 .IsUnicode(true)
                 .HasColumnType("nvarchar(max)");

                e.Property(s => s.LanguageLevel).HasConversion<string>().HasMaxLength(10).IsRequired();
                e.Property(s => s.StoryLanguage).HasConversion<string>().HasMaxLength(50).IsRequired();

                e.Property(s => s.CreatedAt).IsRequired();

                e.HasMany(s => s.Sentences)
                 .WithOne()
                 .HasForeignKey(s => s.StoryId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(s => s.Units)
                 .WithOne()
                 .HasForeignKey(u => u.StoryId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(s => s.CreatedAt);
                e.HasIndex(s => new { s.LanguageLevel, s.StoryLanguage, s.CreatedAt });
            });

            // -------- Sentence (composite key) --------
            modelBuilder.Entity<Sentence>(e =>
            {
                e.HasKey(s => new { s.Id, s.StoryId });
                e.Property(s => s.Id).HasMaxLength(64).IsUnicode(false);
                e.Property(s => s.Text).IsUnicode(true);
            });

            // -------- Unit (composite key + owned Segments + Pieces list) --------
            modelBuilder.Entity<Unit>(e =>
            {
                e.HasKey(u => new { u.Id, u.StoryId });
                e.Property(u => u.Id).HasMaxLength(64).IsUnicode(false);
                e.Property(u => u.IsDiscontinuous).IsRequired();

                // Owned Segments stored in separate table
                e.OwnsMany(u => u.Segments, b =>
                {
                    b.ToTable("Unit_Segments");
                    b.WithOwner().HasForeignKey("UnitId", "StoryId");
                    b.Property<int>("Id");
                    b.HasKey("Id", "UnitId", "StoryId");
                    b.Property(p => p.StartChar);
                    b.Property(p => p.EndChar);
                });

                const char SEP = '|';
                e.Property(u => u.Pieces)
                 .IsUnicode(true)
                 .HasColumnType("nvarchar(max)")
                 .HasConversion(
                     v => v == null ? "" : string.Join(SEP, v),
                     s => string.IsNullOrEmpty(s)
                            ? new List<string>()
                            : new List<string>(s.Split(new[] { SEP }, StringSplitOptions.None))
                 )
                 .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                     (a, b) => a != null && b != null && a.SequenceEqual(b),
                     a => a == null ? 0 : string.Join("|", a).GetHashCode(),
                     a => a == null ? new List<string>() : new List<string>(a)
                 ));
            });

          
        }
    }
}
