using CoreLayer.DTOs;

namespace CoreLayer.ServiceContracts
{
    public interface IUserPreferencesService
    {
        Task<UserPreferencesResponseDto?> GetUserPreferencesAsync(Guid userId);
        Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);
        Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);

        Task AddToSeenStoriesAsync(Guid userId, Guid storyId);
        Task AddToSavedStoriesAsync(Guid userId, Guid storyId);
    }
}