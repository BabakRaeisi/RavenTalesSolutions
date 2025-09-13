using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    // inputs for generation (same as you had, just shown here for completeness)
    public record StoryRequestDto(
        [property: Required]
        [property: EnumDataType(typeof(LanguageLevel))]
        LanguageLevel LanguageLevel,

        [property: Required]
        [property: EnumDataType(typeof(TargetLanguage))]
        TargetLanguage TargetLanguage,

        [property: StringLength(255)]
        string? Topic
    )
    {
        public StoryRequestDto() : this(default, default, default) { }
    }
}
