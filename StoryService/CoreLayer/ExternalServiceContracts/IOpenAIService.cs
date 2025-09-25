using RavenTales.Shared;
using CoreLayer.Entities;

namespace CoreLayer.ExternalServiceContracts
{
    public interface IOpenAIService
    {
        Task<Story> GenerateStoryAsync(LanguageLevel level, Language StoryLanguage, string? topic = null);
    }
}