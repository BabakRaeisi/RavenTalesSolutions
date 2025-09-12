using CoreLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ServiceContracts
{
    public interface IStoryServices
    {
        // Story methods ONLY
        Task<StoryResponseDto> GenerateStoryAsync(Guid userId, StoryRequestDto request);

        // New method to fetch story from database if based on user request and preferences. in case the story id is listed in users seen story list new story will be generated. 
        Task<StoryResponseDto> FetchStoryBasedOnRequestAsync(Guid userId,StoryRequestDto requestDto );
        Task<IEnumerable<StoryResponseDto>> GetUserStoriesAsync(Guid userId);
        Task<StoryResponseDto?> GetStoryByIdAsync(Guid storyId, Guid userId);
        Task<bool> SaveStoryAsync(StoryResponseDto generatedStory, Guid userId);
        Task<bool> DeleteStoryAsync(Guid storyId, Guid userId);

        // Get stories saved/bookmarked by user
        Task<IEnumerable<StoryResponseDto>> GetSavedStoriesAsync(Guid userId);
        
        // Get stories seen by user
        Task<IEnumerable<StoryResponseDto>> GetSeenStoriesAsync(Guid userId);
        
        // Toggle bookmark status
        Task<bool> ToggleBookmarkAsync(Guid userId, Guid storyId);
    }
}
