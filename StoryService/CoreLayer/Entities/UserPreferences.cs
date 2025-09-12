using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.Entities
{
    public class UserPreferences
    {
        [Key]
        public Guid UserId { get; set; }
        
        [Required]
        public LanguageLevel LastUsedLanguageLevel { get; set; }
        
        [Required]
        public TargetLanguage LastUsedTargetLanguage { get; set; }
        
        [Required]
        public TargetLanguage PreferredTranslationLanguage { get; set; }
        
        public List<Guid> SeenStoryIds { get; set; } = new();
        
        public List<Guid> SavedStoryIds { get; set; } = new();
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

 