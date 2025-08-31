using CoreLayer.DTOs;

namespace CoreLayer.StoryContracts
{
    public interface IUserPreferencesService
    {
        Task<UserPreferencesResponseDto?> GetUserPreferencesAsync(Guid userId);
        Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);
        Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request);
    }
}