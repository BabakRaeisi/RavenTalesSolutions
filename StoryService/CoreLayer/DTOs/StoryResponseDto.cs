using CoreLayer.Enums;
using System;

namespace CoreLayer.DTOs
{
    public record StoryResponseDto(Guid Id,Guid userID, string Title ,string Content , LanguageLevel LanguageLevel, TargetLanguage TargetLanguage,string Genre,DateTime CreatedAt)
    {
        public StoryResponseDto() : this(default, default, default, default, default,default,default, default)
        {
            // Default constructor initializes with default values
        }
    }
}
