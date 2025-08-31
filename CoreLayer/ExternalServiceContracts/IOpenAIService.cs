using CoreLayer.Enums;
using CoreLayer.Entities;

namespace CoreLayer.ExternalServiceContracts
{
    public interface IOpenAIService
    {
        Task<Story> GenerateStoryAsync(LanguageLevel level, TargetLanguage language, string? topic = null);
    }
}