using CoreLayer.Entities;
using CoreLayer.Exceptions;
using CoreLayer.RepositoryContracts;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<UserPreferences?> GetUserByIdAsync(Guid userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserPreferences> UpsertUserAsync(UserPreferences userPreferences)
        {
            var existing = await _context.UserPreferences
                .FirstOrDefaultAsync(u => u.UserId == userPreferences.UserId);

            if (existing is null)
            {
                await _context.UserPreferences.AddAsync(userPreferences);
                await _context.SaveChangesAsync();
                return userPreferences; // newly inserted
            }

            existing.LastUsedLanguageLevel = userPreferences.LastUsedLanguageLevel;
            existing.LastUsedTargetLanguage = userPreferences.LastUsedTargetLanguage;
            existing.PreferredTranslationLanguage = userPreferences.PreferredTranslationLanguage;
            existing.SeenStoryIds = userPreferences.SeenStoryIds;
            existing.SavedStoryIds = userPreferences.SavedStoryIds;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing; // return the tracked, updated entity
        }

        public async Task AddStoryToUserHistoryAsync(Guid userId, Guid storyId)
        {
            var prefs = await GetUserByIdAsync(userId);
            if (prefs is null) throw new PreferencesMissingException(userId);

            prefs.SeenStoryIds ??= new List<Guid>();
            if (!prefs.SeenStoryIds.Contains(storyId))
            {
                prefs.SeenStoryIds.Add(storyId);
                prefs.UpdatedAt = DateTime.UtcNow;
                _context.UserPreferences.Update(prefs);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddStoryToUserSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var prefs = await GetUserByIdAsync(userId);
            if (prefs is null) throw new PreferencesMissingException(userId);

            prefs.SavedStoryIds ??= new List<Guid>();
            if (!prefs.SavedStoryIds.Contains(storyId))
            {
                prefs.SavedStoryIds.Add(storyId);
                prefs.UpdatedAt = DateTime.UtcNow;
                _context.UserPreferences.Update(prefs);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromUserSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await GetUserByIdAsync(userId);

            if (preferences?.SavedStoryIds != null && preferences.SavedStoryIds.Contains(storyId))
            {
                preferences.SavedStoryIds.Remove(storyId);
                preferences.UpdatedAt = DateTime.UtcNow;
                _context.UserPreferences.Update(preferences);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Story>> GetUserSavedStories(Guid userId)
        {
            var prefs = await GetUserByIdAsync(userId);
            if (prefs?.SavedStoryIds is null || !prefs.SavedStoryIds.Any())
                return Enumerable.Empty<Story>();

            return await _context.Stories
                .Where(s => prefs.SavedStoryIds.Contains(s.Id))
                .Include(s => s.Sentences)
                .Include(s => s.Units)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetUserStoryHistoryAsync(Guid userId)
        {
            var prefs = await GetUserByIdAsync(userId);
            if (prefs?.SeenStoryIds is null || !prefs.SeenStoryIds.Any())
                return Enumerable.Empty<Story>();

            return await _context.Stories
                .Where(s => prefs.SeenStoryIds.Contains(s.Id))
                .Include(s => s.Sentences)
                .Include(s => s.Units)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
 