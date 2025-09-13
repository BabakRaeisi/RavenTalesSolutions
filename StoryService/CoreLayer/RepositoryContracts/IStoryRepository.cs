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
        Task<Story?> GetStoryById(Guid storyId);
        Task<IEnumerable<Story>> FindStoriesByFilterAsync(LanguageLevel languageLevel,TargetLanguage targetLanguage,string? topic = null);
            
         
    }
}