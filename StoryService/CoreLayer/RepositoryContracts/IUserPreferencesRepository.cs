using CoreLayer.Entities;

namespace CoreLayer.RepositoryContracts
{
    public interface IUserPreferencesRepository
    {
        Task<UserPreferences?> GetByUserIdAsync(Guid userId);
        Task<UserPreferences> UpsertAsync(UserPreferences userPreferences);
        
        // Add specific methods for managing saved/seen stories
        Task AddToSeenStoriesAsync(Guid userId, Guid storyId);
        Task AddToSavedStoriesAsync(Guid userId, Guid storyId);
      
        Task RemoveFromSavedStoriesAsync(Guid userId, Guid storyId);
    }
}