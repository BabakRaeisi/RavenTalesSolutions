using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.Enums;
using CoreLayer.RepositoryContracts;
using CoreLayer.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
        {
            _userPreferencesRepository = userPreferencesRepository;
        }

        public async Task<UserPreferencesResponseDto?> GetUserPreferencesAsync(Guid userId)
        {
            var preferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            if (preferences == null)
                return null;

            return new UserPreferencesResponseDto(
                preferences.UserId,
                preferences.LastUsedLanguageLevel,
                preferences.LastUsedTargetLanguage,
                preferences.CreatedAt,
                preferences.UpdatedAt
            );
        }

        public async Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request)
        {
            var existingPreferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            
            if (existingPreferences == null)
                return await CreateUserPreferencesAsync(userId, request);

            existingPreferences.LastUsedLanguageLevel = request.LanguageLevel;
            existingPreferences.LastUsedTargetLanguage = request.TargetLanguage;
            existingPreferences.PreferredTranslationLanguage = request.PreferredTranslationLanguage;
            existingPreferences.UpdatedAt = DateTime.UtcNow;

            var updatedPreferences = await _userPreferencesRepository.UpsertAsync(existingPreferences);

            return new UserPreferencesResponseDto(
                updatedPreferences.UserId,
                updatedPreferences.LastUsedLanguageLevel,
                updatedPreferences.LastUsedTargetLanguage,
                updatedPreferences.CreatedAt,
                updatedPreferences.UpdatedAt
            );
        }

        public async Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request)
        {
            var newPreferences = new UserPreferences
            {
                UserId = userId,
                LastUsedLanguageLevel = request.LanguageLevel,
                LastUsedTargetLanguage = request.TargetLanguage,
                PreferredTranslationLanguage = request.PreferredTranslationLanguage,
                SeenStoryIds = new List<Guid>(),
                SavedStoryIds = new List<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPreferences = await _userPreferencesRepository.UpsertAsync(newPreferences);

            return new UserPreferencesResponseDto(
                createdPreferences.UserId,
                createdPreferences.LastUsedLanguageLevel,
                createdPreferences.LastUsedTargetLanguage,
                createdPreferences.CreatedAt,
                createdPreferences.UpdatedAt
            );
        }

        public async Task AddToSeenStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await _userPreferencesRepository.GetByUserIdAsync(userId);

            if (preferences == null)
            {
                // Create new preferences if none exist
                preferences = new UserPreferences
                {
                    UserId = userId,
                    LastUsedLanguageLevel = LanguageLevel.B1, // Default
                    LastUsedTargetLanguage = TargetLanguage.English, // Default
                    PreferredTranslationLanguage = TargetLanguage.English, // Default
                    SeenStoryIds = new List<Guid> { storyId },
                    SavedStoryIds = new List<Guid>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            else
            {
                // Initialize if null
                preferences.SeenStoryIds ??= new List<Guid>();

                // Add if not already in list
                if (!preferences.SeenStoryIds.Contains(storyId))
                {
                    preferences.SeenStoryIds.Add(storyId);
                    preferences.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _userPreferencesRepository.UpsertAsync(preferences);
        }

        public async Task AddToSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var preferences = await _userPreferencesRepository.GetByUserIdAsync(userId);

            if (preferences == null)
            {
                // Create new preferences if none exist
                preferences = new UserPreferences
                {
                    UserId = userId,
                    LastUsedLanguageLevel = LanguageLevel.B1, // Default
                    LastUsedTargetLanguage = TargetLanguage.English, // Default
                    PreferredTranslationLanguage = TargetLanguage.English, // Default
                    SeenStoryIds = new List<Guid>(),
                    SavedStoryIds = new List<Guid> { storyId },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            else
            {
                // Initialize if null
                preferences.SavedStoryIds ??= new List<Guid>();

                // Add if not already in list
                if (!preferences.SavedStoryIds.Contains(storyId))
                {
                    preferences.SavedStoryIds.Add(storyId);
                    preferences.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _userPreferencesRepository.UpsertAsync(preferences);
        }
    }
}