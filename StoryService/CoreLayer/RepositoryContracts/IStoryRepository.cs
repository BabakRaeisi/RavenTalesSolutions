using CoreLayer.Entities;
using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayer.RepositoryContracts
{
    public interface IStoryRepository
    {
        Task<Story> AddAsync(Story story);
        Task<Story?> GetByIdAsync(Guid storyId);
        Task<IEnumerable<Story>> GetByUserIdAsync(Guid userId);
        Task<bool> DeleteAsync(Guid storyId);
  
        // New method to find stories by criteria
        Task<IEnumerable<Story>> FindStoriesByFilterAsync(LanguageLevel languageLevel,TargetLanguage targetLanguage,string? topic = null);
            
        // New method to get stories by a list of IDs
        Task<IEnumerable<Story>> GetStoriesByIdsAsync(IEnumerable<Guid> storyIds);
    }
}