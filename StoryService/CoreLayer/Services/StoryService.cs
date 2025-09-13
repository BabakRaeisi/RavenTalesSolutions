using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.ServiceContracts;
using CoreLayer.RepositoryContracts;
using CoreLayer.ExternalServiceContracts;
using CoreLayer.Exceptions;
using CoreLayer.Helpers;
using AutoMapper;
using CoreLayer.Enums;
using System.Linq;

namespace CoreLayer.Services
{
    public class StoryService : IStoryServices
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;

        public StoryService(
            IStoryRepository storyRepository,
            IUserPreferencesRepository userPreferencesRepository,
            IUserPreferencesService userPreferencesService,
            IOpenAIService openAIService,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _userPreferencesService = userPreferencesService;
            _openAIService = openAIService;
            _mapper = mapper;
        }


        public async Task<StoryResponseDto> GenerateStoryAsync(Guid userId, StoryRequestDto request)
        {
            // try DB first
            var cached = await FetchStoryBasedOnRequestAsync(userId, request);
            if (cached is not null) return cached;

            // ensure prefs exist before spending tokens
            var prefs = await _userPreferencesRepository.GetUserByIdAsync(userId);
            if (prefs is null) throw new PreferencesMissingException(userId);

            try
            {
                var story = await _openAIService.GenerateStoryAsync(
                    request.LanguageLevel,
                    request.TargetLanguage,
                    request.Topic);

                story.Id = Guid.NewGuid();
                story.CreatedAt = DateTime.UtcNow;

                StoryValidator.EnsureValidIds(story);
                StoryValidator.ValidateStoryStructure(story);

                await _storyRepository.AddAsync(story);
                await _userPreferencesService.AddStoryToUserHistoryAsync(userId, story.Id);

                return _mapper.Map<StoryResponseDto>(story);
            }
            catch (Exception ex)
            {
                throw new StoryGenerationException($"Failed to generate story for user {userId}", ex);
            }
        }

        public async Task<StoryResponseDto> FetchStoryBasedOnRequestAsync(Guid userId, StoryRequestDto requestDto)
        {
            var userPreferences = await _userPreferencesRepository.GetUserByIdAsync(userId);

            if (userPreferences is null)
                throw new PreferencesMissingException(userId); // <- no silent create

            var matchingStories = await _storyRepository.FindStoriesByFilterAsync(
                requestDto.LanguageLevel,
                requestDto.TargetLanguage,
                requestDto.Topic
            );

            var unseen = StoryHelper.FilterUnseenStories(matchingStories, userPreferences).ToList();
            if (unseen.Count == 0) return null;

            // pick a candidate (freshest or random — your call). here's “freshest”:
            var pick = unseen.OrderByDescending(s => s.CreatedAt).First();

            // mark as seen (since we’re serving it now)
            await _userPreferencesService.AddStoryToUserHistoryAsync(userId, pick.Id);

            return _mapper.Map<StoryResponseDto>(pick);
        }

        public async Task<StoryResponseDto> GetStoryByIdAsync(Guid storyId)
        {
            var story = await _storyRepository.GetStoryById(storyId);
            
     
            if (story == null)
                return null;

            return _mapper.Map<StoryResponseDto>(story);
        }

        // Save the generated story to the database
        public async Task<bool> SaveStoryAsync(StoryResponseDto generatedStory)
        {
            // Convert DTO back to entity and save to database
            var story = _mapper.Map<Story>(generatedStory);
        
            
            // Validate the story structure using the helper class
            StoryValidator.EnsureValidIds(story);
            StoryValidator.ValidateStoryStructure(story);
            
            await _storyRepository.AddAsync(story); // ADD, not UPDATE!
            return true;
        }

 
    }
}