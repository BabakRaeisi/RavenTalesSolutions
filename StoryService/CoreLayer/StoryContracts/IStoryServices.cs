using CoreLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.StoryContracts
{
    public interface IStoryServices
    {
        // Story methods ONLY
        Task<StoryResponseDto> GenerateStoryAsync(Guid userId, StoryRequestDto request);
        Task<IEnumerable<StoryResponseDto>> GetUserStoriesAsync(Guid userId);
        Task<StoryResponseDto?> GetStoryByIdAsync(Guid storyId, Guid userId);
        Task<bool> SaveStoryAsync(StoryResponseDto generatedStory, Guid userId);
        Task<bool> DeleteStoryAsync(Guid storyId, Guid userId);
    }
}
