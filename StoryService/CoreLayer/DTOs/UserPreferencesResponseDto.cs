using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public sealed record UserPreferencesResponseDto(
        Guid UserId,
        LanguageLevel LastUsedLanguageLevel,
        TargetLanguage LastUsedTargetLanguage,
        TargetLanguage PreferredTranslationLanguage,
        IReadOnlyList<Guid> SeenStoryIds,
        IReadOnlyList<Guid> SavedStoryIds,
        DateTime CreatedAt,
        DateTime UpdatedAt
    )
    {
        public UserPreferencesResponseDto() : this(
            default, default, default, default,
            Array.Empty<Guid>(), Array.Empty<Guid>(),
            default, default)
        { }
    }
}