using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever(); // We generate GUIDs in application
                
            builder.Property(s => s.UserId)
                .IsRequired();
                
            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(255);
                
            builder.Property(s => s.Content)
                .IsRequired();
                
            builder.Property(s => s.LanguageLevel)
                .IsRequired()
                .HasConversion<string>();
                
            builder.Property(s => s.TargetLanguage)
                .IsRequired()
                .HasConversion<string>();
                
            builder.Property(s => s.CreatedAt)
                .IsRequired();
                
            builder.Property(s => s.IsBookmarked)
                .IsRequired()
                .HasDefaultValue(false);

            // Configure relationship
            builder.HasOne(s => s.UserPreferences)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(s => s.UserId);
            builder.HasIndex(s => s.CreatedAt);
        }
    }
}