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
         
            StoryResponseDto FetchedUnseen =  await FetchStoryBasedOnRequestAsync(userId, request);
            if (FetchedUnseen != null)
            {
                return FetchedUnseen;
            }
            else
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
                 

                    // Validate the story structure using the helper class
                    StoryValidator.EnsureValidIds(story);
                    StoryValidator.ValidateStoryStructure(story);

                    // Save the generated story
                    await _storyRepository.AddAsync(story);

                    // Mark as seen - using the UserPreferencesService
                    await _userPreferencesService.AddToSeenStoriesAsync(userId, story.Id);

                    return _mapper.Map<StoryResponseDto>(story);
                }
                catch (Exception ex)
                {
                    throw new StoryGenerationException($"Failed to generate story for user {userId}", ex);
                }
            }
          
        }

   
        public async Task<StoryResponseDto> FetchStoryBasedOnRequestAsync(Guid userId, StoryRequestDto requestDto)
        {
            // Get user preferences to check for seen stories
            var userPreferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            
            // Get stories matching the request criteria
            var matchingStories = await _storyRepository.FindStoriesByFilterAsync(
                requestDto.LanguageLevel,
                requestDto.TargetLanguage,
                requestDto.Topic);
            
            // Filter out stories the user has already seen
            var unseenStories = StoryHelper.FilterUnseenStories(matchingStories, userPreferences);
            
          
            return _mapper.Map<StoryResponseDto>(unseenStories);
           

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
            
            // Validate the story structure using the helper class
            StoryValidator.EnsureValidIds(story);
            StoryValidator.ValidateStoryStructure(story);
            
            await _storyRepository.AddAsync(story); // ADD, not UPDATE!
            return true;
        }

        // Get all stories saved/bookmarked by user
        public async Task<IEnumerable<StoryResponseDto>> GetSavedStoriesAsync(Guid userId)
        {
            var userPreferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            
            if (userPreferences?.SavedStoryIds == null || !userPreferences.SavedStoryIds.Any())
            {
                return Enumerable.Empty<StoryResponseDto>();
            }
            
            var savedStories = await _storyRepository.GetStoriesByIdsAsync(userPreferences.SavedStoryIds);
            return _mapper.Map<IEnumerable<StoryResponseDto>>(savedStories);
        }
        
        // Get all stories seen by user
        public async Task<IEnumerable<StoryResponseDto>> GetSeenStoriesAsync(Guid userId)
        {
            var userPreferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            
            if (userPreferences?.SeenStoryIds == null || !userPreferences.SeenStoryIds.Any())
            {
                return Enumerable.Empty<StoryResponseDto>();
            }
            
            var seenStories = await _storyRepository.GetStoriesByIdsAsync(userPreferences.SeenStoryIds);
            return _mapper.Map<IEnumerable<StoryResponseDto>>(seenStories);
        }
        
        // Toggle bookmark status of a story
        public async Task<bool> ToggleBookmarkAsync(Guid userId, Guid storyId)
        {
            var userPreferences = await _userPreferencesRepository.GetByUserIdAsync(userId);
            if (userPreferences == null)
            {
                // Create new user preferences if not exists
                userPreferences = new UserPreferences
                {
                    UserId = userId,
                    SavedStoryIds = new List<Guid> { storyId },
                    SeenStoryIds = new List<Guid>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _userPreferencesRepository.AddToSavedStoriesAsync(userId,storyId);
                
                // Update story bookmark status
                var story = await _storyRepository.GetByIdAsync(storyId);
            
                
                return true;
            }
            
            // Initialize collections if null
            userPreferences.SavedStoryIds ??= new List<Guid>();
            
            if (userPreferences.SavedStoryIds.Contains(storyId))
            {
                // Remove bookmark
                userPreferences.SavedStoryIds.Remove(storyId);
                userPreferences.UpdatedAt = DateTime.UtcNow;
                
                // Update story bookmark status
                var story = await _storyRepository.GetByIdAsync(storyId);
                if (story != null)
                {
              
                }
            }
            else
            {
                // Add bookmark
                userPreferences.SavedStoryIds.Add(storyId);
                userPreferences.UpdatedAt = DateTime.UtcNow;
                
                // Update story bookmark status
                var story = await _storyRepository.GetByIdAsync(storyId);
            
            }
 
            return true;
        }
    }
}