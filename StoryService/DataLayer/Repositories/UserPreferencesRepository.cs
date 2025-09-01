using CoreLayer.Entities;
using CoreLayer.RepositoryContracts;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class UserPreferencesRepository : IUserPreferencesRepository
    {
        private readonly StoryDbContext _context;

        public UserPreferencesRepository(StoryDbContext context)
        {
            _context = context;
        }

        public async Task<UserPreferences?> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserPreferences.FindAsync(userId);
        }

        public async Task<UserPreferences> UpsertAsync(UserPreferences userPreferences)
        {
            var existing = await _context.UserPreferences.FindAsync(userPreferences.UserId);
            
            if (existing == null)
            {
                // Create new
                await _context.UserPreferences.AddAsync(userPreferences);
            }
            else
            {
                // Update existing
                existing.LastUsedLanguageLevel = userPreferences.LastUsedLanguageLevel;
                existing.LastUsedTargetLanguage = userPreferences.LastUsedTargetLanguage;
                existing.UpdatedAt = userPreferences.UpdatedAt;
                _context.UserPreferences.Update(existing);
            }

            await _context.SaveChangesAsync();
            return userPreferences;
        }
    }
}