using CoreLayer.DTOs;

namespace CoreLayer.ServiceContracts
{
    public interface IUserPreferencesService
    {
        Task<UserPreferencesResponseDto?> GetUserByIdAsync(Guid userId);
        Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);
        Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);
      
        Task<IEnumerable<StoryResponseDto>> GetUserSavedStoriesAsync(Guid userId);
        Task<IEnumerable<StoryResponseDto>> GetUserStoryHistoryAsync(Guid userId);

        Task AddStoryToUserHistoryAsync(Guid userId, Guid storyId);
        Task AddToUserSavedStoriesAsync(Guid userId, Guid storyId);

        Task RemoveFromUserSavedStoriesAsync(Guid userId, Guid storyId);
    }
}