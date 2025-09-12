using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public record UserPreferencesResponseDto(Guid userId,LanguageLevel LanguageLevel , TargetLanguage TargetLanguage,DateTime createdAt, DateTime updatedAt)
    {
     public UserPreferencesResponseDto() : this(default, default, default, default, default)
        {
            // Default constructor initializes with default values
        }
    }
}