using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    // mirrors your domain Story + the agreed Sentences/Units
    public sealed record StoryResponseDto(
        Guid Id,
        [property: Required] string Title,
        [property: Required] string Content,
        [property: Required] string Genre,
        [property: Required] LanguageLevel LanguageLevel,
        [property: Required] TargetLanguage TargetLanguage,
        DateTime CreatedAt,
        [property: Required] IReadOnlyList<SentenceDto> Sentences,
        [property: Required] IReadOnlyList<UnitDto> Units
    )
    {
        public StoryResponseDto() : this(
            default, "", "", "", default, default, default,
            Array.Empty<SentenceDto>(), Array.Empty<UnitDto>())
        { }
    }
}
