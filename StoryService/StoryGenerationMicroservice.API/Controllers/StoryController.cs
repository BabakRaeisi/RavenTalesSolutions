using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoreLayer.ServiceContracts;
using CoreLayer.DTOs;
using StoryGenerationMicroservice.API.Extensions;

namespace StoryGenerationMicroservice.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryServices _storyService;
        private readonly ILogger<StoryController> _logger;

        public StoryController(IStoryServices storyService, ILogger<StoryController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<StoryResponseDto>> GenerateStory([FromBody] StoryRequestDto request)
        {
            try
            {
                var userId = User.GetUserId();
                var story = await _storyService.GenerateStoryAsync(userId, request);
                return Ok(story);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate story");
                return StatusCode(500, new { message = "Failed to generate story" });
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