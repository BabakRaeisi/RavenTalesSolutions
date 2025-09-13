using System;
using System.Collections.Generic;
using System.Linq;
using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataLayer.DbContext
{
    public class StoryDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options) { }

        public DbSet<Story> Stories { get; set; } = null!;
        public DbSet<UserPreferences> UserPreferences { get; set; } = null!;
        public DbSet<Sentence> Sentences { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Story ----------
            modelBuilder.Entity<Story>(entity =>
            {
                entity.HasMany(s => s.Sentences)
                      .WithOne()
                      .HasForeignKey(s => s.StoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Units)
                      .WithOne()
                      .HasForeignKey(u => u.StoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(s => s.Title).IsUnicode(true);
                entity.Property(s => s.Genre).IsUnicode(true);
                entity.Property(s => s.Content)
                      .IsUnicode(true)
                      .HasColumnType("nvarchar(max)");

                // enums as strings
                entity.Property(s => s.LanguageLevel).HasConversion<string>().IsRequired();
                entity.Property(s => s.TargetLanguage).HasConversion<string>().IsRequired();

                // indexes (keep both the legacy singles and our composite)
                
                entity.HasIndex(s => s.CreatedAt);
                entity.HasIndex(s => new { s.LanguageLevel, s.TargetLanguage, s.CreatedAt });
            });

            // ---------- Sentence ----------
            modelBuilder.Entity<Sentence>(entity =>
            {
                entity.HasKey(s => new { s.Id, s.StoryId });

                // key parts must be bounded (not nvarchar(max))
                entity.Property(s => s.Id)
                      .HasMaxLength(32)
                      .IsUnicode(false);

                entity.Property(s => s.Text).IsUnicode(true);
            });

            // ---------- Unit ----------
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(u => new { u.Id, u.StoryId });

                entity.Property(u => u.Id)
                      .HasMaxLength(32)
                      .IsUnicode(false);

                // owned Segments rows linked by (UnitId, StoryId)
                entity.OwnsMany(u => u.Segments, b =>
                {
                    b.WithOwner().HasForeignKey("UnitId", "StoryId");
                    b.Property<int>("Id");               // surrogate key per owned row
                    b.HasKey("Id", "UnitId", "StoryId");
                    b.Property(p => p.StartChar);
                    b.Property(p => p.EndChar);
                });

                // Pieces as Unicode nvarchar(max) with simple '|' join
                const char SEP = '|';
                entity.Property(u => u.Pieces)
                      .IsUnicode(true)
                      .HasColumnType("nvarchar(max)")
                      .HasConversion(
                          v => v == null ? "" : string.Join(SEP, v),
                          s => string.IsNullOrEmpty(s)
                                ? new List<string>()
                                : new List<string>(s.Split(new[] { SEP }, StringSplitOptions.None))
                      )
                      // ValueComparer to track list changes & silence warnings
                      .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                          (a, b) => a != null && b != null && a.SequenceEqual(b),
                          a => a == null ? 0 : string.Join("|", a).GetHashCode(),
                          a => a == null ? new List<string>() : new List<string>(a)
                      ));
            });

            // ---------- UserPreferences ----------
            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.HasKey(u => u.UserId);

                // enums as strings
                entity.Property(u => u.LastUsedLanguageLevel).HasConversion<string>().IsRequired();
                entity.Property(u => u.LastUsedTargetLanguage).HasConversion<string>().IsRequired();
                entity.Property(u => u.PreferredTranslationLanguage).HasConversion<string>().IsRequired();

                // SeenStoryIds as CSV of GUIDs
                entity.Property(u => u.SeenStoryIds)
                      .HasConversion(
                          v => v == null ? "" : string.Join(',', v),
                          s => string.IsNullOrWhiteSpace(s)
                                ? new List<Guid>()
                                : s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(Guid.Parse).ToList()
                      )
                      .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                          (a, b) => a != null && b != null && a.SequenceEqual(b),
                          a => a == null ? 0 : string.Join(",", a).GetHashCode(),
                          a => a == null ? new List<Guid>() : new List<Guid>(a)
                      ));

                // SavedStoryIds as CSV of GUIDs
                entity.Property(u => u.SavedStoryIds)
                      .HasConversion(
                          v => v == null ? "" : string.Join(',', v),
                          s => string.IsNullOrWhiteSpace(s)
                                ? new List<Guid>()
                                : s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(Guid.Parse).ToList()
                      )
                      .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                          (a, b) => a != null && b != null && a.SequenceEqual(b),
                          a => a == null ? 0 : string.Join(",", a).GetHashCode(),
                          a => a == null ? new List<Guid>() : new List<Guid>(a)
                      ));
            });
        }
    }
}
