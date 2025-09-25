using CoreLayer.Entities;
using RavenTales.Shared;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace CoreLayer.RepositoryContracts
{
    public interface IStoryRepository
    {
        Task<Story> AddAsync(Story story);
        Task<Story?> GetStoryById(Guid storyId , CancellationToken ct = default);
        Task<IEnumerable<Story>> FindStoriesByFilterAsync(LanguageLevel languageLevel,Language targetLanguage,string? topic = null , CancellationToken ct = default);
        Task<IEnumerable<Story>> QueryMatchingStoriesAsync(Language language, LanguageLevel level, string? topic ,CancellationToken ct = default);

    }
}