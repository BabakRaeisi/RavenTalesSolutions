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
        Task<StoryResponseDto> FetchStoryBasedOnRequestAsync(Guid userId,StoryRequestDto requestDto );
        Task<StoryResponseDto?> GetStoryByIdAsync(Guid storyId);
        Task<bool> SaveStoryAsync(StoryResponseDto generatedStory);

        
    }
}
