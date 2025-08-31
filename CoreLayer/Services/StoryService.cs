using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.StoryContracts;
using CoreLayer.RepositoryContracts;
using CoreLayer.ExternalServiceContracts;
using CoreLayer.Exceptions;
using AutoMapper;
using CoreLayer.Enums;

namespace CoreLayer.Services
{
    public class StoryService : IStoryServices
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;

        public StoryService(
            IStoryRepository storyRepository,
            IUserPreferencesRepository userPreferencesRepository,
            IOpenAIService openAIService,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _openAIService = openAIService;
            _mapper = mapper;
        }

        public async Task<StoryResponseDto> GenerateStoryAsync(Guid userId, StoryRequestDto request)
        {
            try
            {
                var story = await _openAIService.GenerateStoryAsync(
                    request.LanguageLevel,
                    request.TargetLanguage,
                    request.Topic);
                
                story.Id = Guid.NewGuid();
                story.UserId = userId;
                story.CreatedAt = DateTime.UtcNow;
                story.IsBookmarked = false;

                return _mapper.Map<StoryResponseDto>(story);
            }
            catch (Exception ex)
            {
                throw new StoryGenerationException($"Failed to generate story for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<StoryResponseDto>> GetUserStoriesAsync(Guid userId)
        {
            var stories = await _storyRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<StoryResponseDto>>(stories);
        }

        public async Task<StoryResponseDto?> GetStoryByIdAsync(Guid storyId, Guid userId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            
            // Ensure user can only access their own stories
            if (story == null || story.UserId != userId)
                return null;

            return _mapper.Map<StoryResponseDto>(story);
        }

        public async Task<bool> DeleteStoryAsync(Guid storyId, Guid userId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            
            // Verify story exists and belongs to user
            if (story == null || story.UserId != userId)
                return false;

            return await _storyRepository.DeleteAsync(storyId);
        }

        // Save the generated story to the database
        public async Task<bool> SaveStoryAsync(StoryResponseDto generatedStory, Guid userId)
        {
            // Convert DTO back to entity and save to database
            var story = _mapper.Map<Story>(generatedStory);
            story.UserId = userId;
            
            await _storyRepository.AddAsync(story); // ADD, not UPDATE!
            return true;
        }
    }
}