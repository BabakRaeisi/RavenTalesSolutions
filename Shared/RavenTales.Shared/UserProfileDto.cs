 
using System.ComponentModel.DataAnnotations;
namespace RavenTales.Shared;
public record UserProfileDto(
    [property: Required] Guid UserId,
    [property: Required] LanguageLevel LanguageLevel,
    [property: Required] Language TargetLanguage,
    [property: Required] Language TranslationLanguage
);