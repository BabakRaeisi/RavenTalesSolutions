using CoreLayer.Entities;

namespace CoreLayer.RepositoryContracts
{
    public interface IUserPreferencesRepository
    {
        Task<UserPreferences?> GetByUserIdAsync(Guid userId);
        Task<UserPreferences> UpsertAsync(UserPreferences userPreferences);
    }
}