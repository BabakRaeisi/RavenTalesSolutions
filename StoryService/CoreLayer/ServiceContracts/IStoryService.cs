using RavenTales.Shared;
using CoreLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ServiceContracts
{
    public interface IStoryService
    {


        Task<StoryCardDto?> GenerateStoryAsync(Guid userId, StoryRequestDto request, CancellationToken ct = default);
        Task<IEnumerable<StoryCardDto?>> FindStoriesByFilterAsync(Guid userId, StoryRequestDto request, CancellationToken ct = default);

        // If you log viewing history or personalize, include userId; otherwise you can drop it.
        Task<StoryResponseDto?> GetStoryByIdAsync(Guid userId, Guid storyId, CancellationToken ct = default);

        // Replace UserProfileDto param with userId; fetch profile internally.
        Task<StoryCardDto?> FetchStoryBasedOnRequestAsync(Guid userId, StoryRequestDto request, CancellationToken ct = default);

        Task<IEnumerable<StoryCardDto?>> GetRecommendedStoriesAsync(Guid userId, CancellationToken ct = default);

        // History (user-scoped)
        Task ClearHistory(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<StoryCardDto?>> GetHistory(Guid userId, CancellationToken ct = default);

        // Saved stories (user-scoped)
        Task<bool> SaveStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default);
        Task<IEnumerable<StoryCardDto?>> GetSavedStories(Guid userId, CancellationToken ct = default);
        Task<bool> RemoveSavedStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default);

        Task SkipStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default);
    }
}
