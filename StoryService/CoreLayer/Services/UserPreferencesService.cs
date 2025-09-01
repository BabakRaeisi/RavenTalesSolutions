using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.StoryContracts;
using CoreLayer.RepositoryContracts;
using CoreLayer.Enums;
using AutoMapper;

namespace CoreLayer.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IMapper _mapper;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository, IMapper mapper)
        {
            _userPreferencesRepository = userPreferencesRepository;
            _mapper = mapper;
        }

        public async Task<UserPreferencesResponseDto?> GetUserPreferencesAsync(Guid userId)
        {
            var preferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            return preferences == null ? null : _mapper.Map<UserPreferencesResponseDto>(preferences);
        }

        public async Task<UserPreferencesResponseDto> CreateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request)
        {
            var preferences = new UserPreferences
            {
                UserId = userId,
                LastUsedLanguageLevel = request.LanguageLevel,
                LastUsedTargetLanguage = request.TargetLanguage,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var savedPreferences = await _userPreferencesRepository.UpsertAsync(preferences);
            return _mapper.Map<UserPreferencesResponseDto>(savedPreferences);
        }

        public async Task<UserPreferencesResponseDto> UpdateUserPreferencesAsync(Guid userId, UserPreferencesUpdateRequestDto request)
        {
            var preferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            
            if (preferences == null)
            {
                return await CreateUserPreferencesAsync(userId, request);
            }

            preferences.LastUsedLanguageLevel = request.LanguageLevel;
            preferences.LastUsedTargetLanguage = request.TargetLanguage;
            preferences.UpdatedAt = DateTime.UtcNow;

            var savedPreferences = await _userPreferencesRepository.UpsertAsync(preferences);
            return _mapper.Map<UserPreferencesResponseDto>(savedPreferences);
        }
    }
}