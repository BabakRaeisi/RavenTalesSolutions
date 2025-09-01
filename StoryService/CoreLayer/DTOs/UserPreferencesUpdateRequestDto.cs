using CoreLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public class UserPreferencesUpdateRequestDto
    {
        [Required]
        [EnumDataType(typeof(LanguageLevel))]
        public LanguageLevel LanguageLevel { get; set; }

        [Required]
        [EnumDataType(typeof(TargetLanguage))]
        public TargetLanguage TargetLanguage { get; set; }

        [Required]
        [EnumDataType(typeof(TargetLanguage))]
        public TargetLanguage PreferredTranslationLanguage { get; set; }
    }
}