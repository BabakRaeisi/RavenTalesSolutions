using CoreLayer.Entities;

namespace CoreLayer.RepositoryContracts
{
    public interface IUserPreferencesRepository
    {
        Task<UserPreferences?> GetUserByIdAsync(Guid userId);
        Task<UserPreferences> UpsertUserAsync(UserPreferences userPreferences);
        
        // Add specific methods for managing saved/seen stories

        Task<IEnumerable<Story>> GetUserSavedStories(Guid UserId);
        Task<IEnumerable<Story>> GetUserStoryHistoryAsync(Guid userId);
        Task AddStoryToUserHistoryAsync(Guid userId, Guid storyId);
        Task AddStoryToUserSavedStoriesAsync(Guid userId, Guid storyId);
      
        Task RemoveFromUserSavedStoriesAsync(Guid userId, Guid storyId);
    }
}