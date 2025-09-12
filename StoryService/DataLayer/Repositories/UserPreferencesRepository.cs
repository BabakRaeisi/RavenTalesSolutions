using CoreLayer.Entities;
using CoreLayer.RepositoryContracts;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.UserPreferences
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserPreferences> UpsertAsync(UserPreferences userPreferences)
        {
            var existingPreferences = await _context.UserPreferences
                .FirstOrDefaultAsync(u => u.UserId == userPreferences.UserId);

            if (existingPreferences == null)
            {
                // Insert new
                await _context.UserPreferences.AddAsync(userPreferences);
            }
            else
            {
                // Update existing
                existingPreferences.LastUsedLanguageLevel = userPreferences.LastUsedLanguageLevel;
                existingPreferences.LastUsedTargetLanguage = userPreferences.LastUsedTargetLanguage;
                existingPreferences.PreferredTranslationLanguage = userPreferences.PreferredTranslationLanguage;
                existingPreferences.SeenStoryIds = userPreferences.SeenStoryIds;
                existingPreferences.SavedStoryIds = userPreferences.SavedStoryIds;
                existingPreferences.UpdatedAt = DateTime.UtcNow;
                
                _context.UserPreferences.Update(existingPreferences);
            }

            await _context.SaveChangesAsync();
            return userPreferences;
        }
        
        public async Task AddToSeenStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await GetByUserIdAsync(userId);
            
            if (preferences == null)
            {
                // Create new preferences with default values
                preferences = new UserPreferences
                {
                    UserId = userId,
                    LastUsedLanguageLevel = CoreLayer.Enums.LanguageLevel.B1,
                    LastUsedTargetLanguage = CoreLayer.Enums.TargetLanguage.English,
                    PreferredTranslationLanguage = CoreLayer.Enums.TargetLanguage.English,
                    SeenStoryIds = new List<Guid> { storyId },
                    SavedStoryIds = new List<Guid>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _context.UserPreferences.AddAsync(preferences);
            }
            else
            {
                // Ensure collection is initialized
                preferences.SeenStoryIds ??= new List<Guid>();
                
                // Add storyId if not already present
                if (!preferences.SeenStoryIds.Contains(storyId))
                {
                    preferences.SeenStoryIds.Add(storyId);
                    preferences.UpdatedAt = DateTime.UtcNow;
                    _context.UserPreferences.Update(preferences);
                }
            }
            
            await _context.SaveChangesAsync();
        }
        
        public async Task AddToSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await GetByUserIdAsync(userId);
            
            if (preferences == null)
            {
                // Create new preferences with default values
                preferences = new UserPreferences
                {
                    UserId = userId,
                    LastUsedLanguageLevel = CoreLayer.Enums.LanguageLevel.B1,
                    LastUsedTargetLanguage = CoreLayer.Enums.TargetLanguage.English,
                    PreferredTranslationLanguage = CoreLayer.Enums.TargetLanguage.English,
                    SeenStoryIds = new List<Guid>(),
                    SavedStoryIds = new List<Guid> { storyId },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _context.UserPreferences.AddAsync(preferences);
            }
            else
            {
                // Ensure collection is initialized
                preferences.SavedStoryIds ??= new List<Guid>();
                
                // Add storyId if not already present
                if (!preferences.SavedStoryIds.Contains(storyId))
                {
                    preferences.SavedStoryIds.Add(storyId);
                    preferences.UpdatedAt = DateTime.UtcNow;
                    _context.UserPreferences.Update(preferences);
                }
            }
            
            await _context.SaveChangesAsync();
        }
 
        public async Task RemoveFromSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await GetByUserIdAsync(userId);
            
            if (preferences?.SavedStoryIds != null && preferences.SavedStoryIds.Contains(storyId))
            {
                preferences.SavedStoryIds.Remove(storyId);
                preferences.UpdatedAt = DateTime.UtcNow;
                _context.UserPreferences.Update(preferences);
                await _context.SaveChangesAsync();
            }
        }
    }
}