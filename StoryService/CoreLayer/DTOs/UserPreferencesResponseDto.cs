using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public class UserPreferencesResponseDto
    {
        public Guid UserId { get; set; }
        public LanguageLevel LastUsedLanguageLevel { get; set; }
        public TargetLanguage LastUsedTargetLanguage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}