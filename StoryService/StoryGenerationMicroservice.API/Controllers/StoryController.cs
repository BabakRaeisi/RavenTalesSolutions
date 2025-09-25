using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoreLayer.ServiceContracts;
using CoreLayer.DTOs;
using RavenTales.Shared;
using StoryGenerationMicroservice.API.Extensions;
using CoreLayer.Exceptions;

namespace StoryGenerationMicroservice.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;

        public StoryController(IStoryService storyService, ILogger<StoryController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<StoryResponseDto>> GenerateStory([FromBody] StoryRequestDto request)
        {
            Guid userId;
            try { userId = User.GetUserId(); }
            catch (UnauthorizedAccessException) { return Unauthorized(); }

            try
            {
                var story = await _storyService.GenerateStoryAsync(userId, request);
                return Ok(story);
            }
          
            catch (StoryGenerationException)
            {
                return StatusCode(502, new { errorCode = "STORY_GENERATION_FAILED", message = "Story generation failed. Please try again." });
            }
        }

        [HttpGet("{storyId:guid}")]
        public async Task<ActionResult<StoryResponseDto>> GetStoryById(Guid storyId)
        {
            try
            {

                var story = await _storyService.GetStoryByIdAsync(storyId);

                if (story == null)
                    return NotFound();

                return Ok(story);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get story {StoryId}", storyId);
                return StatusCode(500, new { message = "Failed to retrieve story" });
            }
        }

    }
}