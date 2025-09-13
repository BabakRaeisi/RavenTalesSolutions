using AutoMapper;
using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.Enums;
using CoreLayer.Exceptions;
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
        private readonly IStoryRepository _storyRepository;
        private readonly  IMapper _mapper ;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository, IStoryRepository storyRepository,IMapper mapper)
        {
            _userPreferencesRepository = userPreferencesRepository;
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public async Task<UserPreferencesResponseDto?> GetUserByIdAsync(Guid userId)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId);
            return prefs is null ? null : _mapper.Map<UserPreferencesResponseDto>(prefs);
        }


        public async Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto req)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId)
                       ?? throw new PreferencesMissingException(userId); // or create here if you want

            prefs.LastUsedLanguageLevel = req.LanguageLevel;
            prefs.LastUsedTargetLanguage = req.TargetLanguage;
            prefs.PreferredTranslationLanguage = req.PreferredTranslationLanguage;
            prefs.UpdatedAt = DateTime.UtcNow;

            var updated = await _userPreferencesRepository.UpsertUserAsync(prefs);
            return _mapper.Map<UserPreferencesResponseDto>(updated);
        }

        public async Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto req)
        {
            var prefs = new UserPreferences
            {
                UserId = userId,
                LastUsedLanguageLevel = req.LanguageLevel,
                LastUsedTargetLanguage = req.TargetLanguage,
                PreferredTranslationLanguage = req.PreferredTranslationLanguage,
                SeenStoryIds = new List<Guid>(),
                SavedStoryIds = new List<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var created = await _userPreferencesRepository.UpsertUserAsync(prefs);
            return _mapper.Map<UserPreferencesResponseDto>(created);
        }

        // Get all stories saved/bookmarked by user
        public async Task<IEnumerable<StoryResponseDto>> GetUserSavedStoriesAsync(Guid userId)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId);
            if (prefs?.SavedStoryIds is null || !prefs.SavedStoryIds.Any()) return Enumerable.Empty<StoryResponseDto>();

            var stories = await _userPreferencesRepository.GetUserSavedStories(userId);
            return _mapper.Map<IEnumerable<StoryResponseDto>>(stories);
        }

        public async Task<IEnumerable<StoryResponseDto>> GetUserStoryHistoryAsync(Guid userId)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId);
            if (prefs?.SeenStoryIds is null || !prefs.SeenStoryIds.Any()) return Enumerable.Empty<StoryResponseDto>();

            var stories = await _userPreferencesRepository.GetUserStoryHistoryAsync(userId);
            return _mapper.Map<IEnumerable<StoryResponseDto>>(stories);
        }

        // Toggle bookmark status of a story

        public async Task AddStoryToUserHistoryAsync(Guid userId, Guid storyId)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId);
            if (prefs is null) throw new PreferencesMissingException(userId);

            prefs.SeenStoryIds ??= new List<Guid>();
            if (!prefs.SeenStoryIds.Contains(storyId))
            {
                prefs.SeenStoryIds.Add(storyId);
                prefs.UpdatedAt = DateTime.UtcNow;
                await _userPreferencesRepository.UpsertUserAsync(prefs);
            }
        }

        public async Task AddToUserSavedStoriesAsync(Guid userId, Guid storyId)
        {
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId)
               ?? throw new PreferencesMissingException(userId);

            prefs.SavedStoryIds ??= new List<Guid>();
            if (prefs.SavedStoryIds.Contains(storyId))
                prefs.SavedStoryIds.Remove(storyId);
            else
                prefs.SavedStoryIds.Add(storyId);

            prefs.UpdatedAt = DateTime.UtcNow;
            await _userPreferencesRepository.UpsertUserAsync(prefs);
        }
         

        public async Task RemoveFromUserSavedStoriesAsync(Guid userId, Guid storyId)
        {
           await _userPreferencesRepository.RemoveFromUserSavedStoriesAsync(userId, storyId);
        }
    }
}