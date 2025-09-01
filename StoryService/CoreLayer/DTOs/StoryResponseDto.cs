using CoreLayer.Enums;
using System;

namespace CoreLayer.DTOs
{
    public class StoryResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public LanguageLevel LanguageLevel { get; set; }
        public TargetLanguage TargetLanguage { get; set; }
        public string Genre { get; set; } = string.Empty; // Added
        public DateTime CreatedAt { get; set; }
        public bool IsBookmarked { get; set; }
    }
}
