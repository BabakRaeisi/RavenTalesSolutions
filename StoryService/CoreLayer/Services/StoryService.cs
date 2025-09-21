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
         
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;

        public StoryService(
            IStoryRepository storyRepository,
             
            IOpenAIService openAIService,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
             
            _openAIService = openAIService;
            _mapper = mapper;
        }

        public Task<StoryResponseDto> FetchStoryBasedOnRequestAsync(Guid userId, StoryRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<StoryResponseDto> GenerateStoryAsync(Guid userId, StoryRequestDto request)
        {
            throw new NotImplementedException();
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