using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.HasKey(u => u.UserId);
            
            builder.Property(u => u.UserId)
                .IsRequired();
                
            builder.Property(u => u.LastUsedLanguageLevel)
                .IsRequired()
                .HasConversion<string>();
                
            builder.Property(u => u.LastUsedTargetLanguage)
                .IsRequired()
                .HasConversion<string>();
                
            builder.Property(u => u.CreatedAt)
                .IsRequired();
                
            builder.Property(u => u.UpdatedAt)
                .IsRequired();

            // Configure relationship
            builder.HasMany(u => u.Stories)
                .WithOne(s => s.UserPreferences)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}