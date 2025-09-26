using CoreLayer.DTOs;
using CoreLayer.Entities;
using CoreLayer.ServiceContracts;
using CoreLayer.RepositoryContracts;
using CoreLayer.ExternalServiceContracts;
 using CoreLayer.Helpers;
using AutoMapper;
using RavenTales.Shared;
using System.Linq;

namespace CoreLayer.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUserStoryHistoryRepository _userHistoryRepository;
        private readonly IUserStoryBookmarkRepository _userSavedStoryRepository;
        private readonly IProfileClient _profileClient;
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;
        private readonly IUserSkipStore _skipStore;

        public StoryService(
            IStoryRepository storyRepository,
            IUserStoryHistoryRepository userStoryHistoryRepository,
            IUserStoryBookmarkRepository userSavedStoryRepository,
            IOpenAIService openAIService,
            IMapper mapper,
            IProfileClient profileClient,
            IUserSkipStore skipStore
            )
        {
            _storyRepository = storyRepository;
            _userHistoryRepository = userStoryHistoryRepository;
            _userSavedStoryRepository = userSavedStoryRepository;
            _profileClient = profileClient;
            _openAIService = openAIService;
            _mapper = mapper;
            _skipStore = skipStore;
        }

        public async Task<StoryCardDto?> GenerateStoryAsync(Guid userId, StoryRequestDto request, CancellationToken ct = default)
        {
            var profile = await _profileClient.GetAsync(userId, ct);

            var shortlist = await FetchStoryBasedOnRequestAsync(userId, request, ct);
            if (shortlist is not null) return shortlist;

            var generated = await _openAIService.GenerateStoryAsync(request.LanguageLevel, request.TargetLanguage, request.Topic);
            if (generated is null) throw new Exception("Story generation failed.");

            await _storyRepository.AddAsync(generated); // your chosen flow
            return _mapper.Map<StoryCardDto>(generated);
        }

        public async Task<IEnumerable<StoryCardDto?>> FindStoriesByFilterAsync(Guid userId, LanguageLevel level ,Language language ,string? topic=null , CancellationToken ct = default)
        {
            IEnumerable<Story?> stories =  await _storyRepository.FindStoriesByFilterAsync(level, language,topic ,ct);
            //if stories are more than 0 return them as of StoryCardDto else return empty list
            if (stories.Any())
                return _mapper.Map<List<StoryCardDto?>>(stories);
            else 
            {
              
                throw new Exception("no stories were found.consider changing your filter");

            }

        }

        public async Task<StoryResponseDto?> GetStoryByIdAsync(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            var story = await _storyRepository.GetStoryById(storyId, ct);
            if (story is null) return null;
            else
            return _mapper.Map<StoryResponseDto>(story);
        }

        public async Task<StoryCardDto?> FetchStoryBasedOnRequestAsync(Guid userId, StoryRequestDto request, CancellationToken ct = default)
        {
            // 1) candidates by lang/level/topic
            var candidates = await _storyRepository.QueryMatchingStoriesAsync(
                request.TargetLanguage, request.LanguageLevel, request.Topic, ct);

            // 2) exclude user history
            var historyIds = (await _userHistoryRepository.List(userId, ct)).Select(s => s.Id).ToHashSet();

            // 3) exclude temp-skips (in-memory)
            var skipped = _skipStore.GetSkipped(userId); // HashSet<Guid>

            var pick = candidates.FirstOrDefault(s => !historyIds.Contains(s.Id) && !skipped.Contains(s.Id));
            return pick is null ? null : _mapper.Map<StoryCardDto>(pick);
        }

        public Task<IEnumerable<StoryCardDto?>> GetRecommendedStoriesAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task ClearHistory(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StoryCardDto?>> GetHistory(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StoryCardDto?>> GetSavedStories(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveSavedStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }


        public Task SkipStoryAsync(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            _skipStore.AddSkip(userId, storyId);
            return Task.CompletedTask;
        }
    }
}