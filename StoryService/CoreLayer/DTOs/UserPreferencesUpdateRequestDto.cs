using CoreLayer.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public record UserPreferencesUpdateRequestDto (
      [property: System.ComponentModel.DataAnnotations.Required]
      [property: System.ComponentModel.DataAnnotations.EnumDataType(typeof(CoreLayer.Enums.LanguageLevel))]
       CoreLayer.Enums.LanguageLevel LanguageLevel,

      [property: System.ComponentModel.DataAnnotations.Required]
      [property: System.ComponentModel.DataAnnotations.EnumDataType(typeof(CoreLayer.Enums.TargetLanguage))]
       CoreLayer.Enums.TargetLanguage TargetLanguage,

      [property: System.ComponentModel.DataAnnotations.Required]
      [property: System.ComponentModel.DataAnnotations.EnumDataType(typeof(CoreLayer.Enums.TargetLanguage))]
       CoreLayer.Enums.TargetLanguage PreferredTranslationLanguage

        )
    {
        public UserPreferencesUpdateRequestDto() : this(default, default, default)
        {
            // Default constructor initializes with default values
        }

    }
}