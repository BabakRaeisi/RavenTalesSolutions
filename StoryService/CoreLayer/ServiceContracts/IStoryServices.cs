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
        Task<StoryCardDto> GenerateStoryAsync(StoryRequestDto request);
        Task<IEnumerable<StoryCardDto>> FindStoriesByFilterAsync(StoryRequestDto requestDto);
        Task<StoryResponseDto?> GetStoryByIdAsync(Guid storyId);
        Task<StoryCardDto> FetchStoryBasedOnRequestAsync(Guid userId,StoryRequestDto requestDto );

        Task<IEnumerable<StoryCardDto>> GetRecommendedStoriesAsync(UserProfileDto profile) 

    }
}
