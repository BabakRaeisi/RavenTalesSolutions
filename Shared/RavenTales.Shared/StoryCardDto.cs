
using System.ComponentModel.DataAnnotations;
namespace RavenTales.Shared
{
    // A lightweight representation of a story for listing purposes
    // Used in story listings, e.g., user's saved stories
    // Does NOT include full content or sentences/units
    // Mirrors key metadata from StoryResponseDto
    public sealed record StoryCardDto(
    [property :Required] Guid StoryId,

    [MaxLength(200)] string? Title,

    [property: Required] LanguageLevel LanguageLevel,
    [property: Required] Language TargetLanguage,

    [MaxLength(100)] string? Topic,

    [Required, MaxLength(200)]
    string TextPreview
)
    { public StoryCardDto() : this(default,"",default,default,"","") { } }
}