using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;
namespace CoreLayer.DTOs
{
    // A lightweight representation of a story for listing purposes
    // Used in story listings, e.g., user's saved stories
    // Does NOT include full content or sentences/units
    // Mirrors key metadata from StoryResponseDto
    public sealed record StoryCardDto(
    [property :Required] Guid StoryId,

    [MaxLength(200)] string? Title,

    [property: Required] LanguageLevel LanguageLevel,
    [property: Required] TargetLanguage TargetLanguage,

    [MaxLength(100)] string? Topic,

    [Required, MaxLength(200)]
    string Excerpt
)
    { public StoryCardDto() : this(default,"",default,default,"","") { } }
}