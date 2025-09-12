using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoreLayer.StoryContracts;
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
        [HttpPost("SaveStory")]
        public async Task<ActionResult<StoryResponseDto>> SaveStory([FromBody] StoryResponseDto request)
        {
            try
            {
                  var userId = User.GetUserId();
                var story = await _storyService.SaveStoryAsync(request, userId);
                return Ok(story);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save story");
                return StatusCode(500, new { message = "Failed to save story" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryResponseDto>>> GetUserStories()
        {
            try
            {
                var userId = User.GetUserId();
                var stories = await _storyService.GetUserStoriesAsync(userId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user stories");
                return StatusCode(500, new { message = "Failed to retrieve stories" });
            }
        }

        [HttpGet("{storyId:guid}")]
        public async Task<ActionResult<StoryResponseDto>> GetStoryById(Guid storyId)
        {
            try
            {
                var userId = User.GetUserId();
                var story = await _storyService.GetStoryByIdAsync(storyId, userId);
                
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

        [HttpDelete("{storyId:guid}")]
        public async Task<IActionResult> DeleteStory(Guid storyId)
        {
            try
            {
                var userId = User.GetUserId();
                var success = await _storyService.DeleteStoryAsync(storyId, userId);
                
                if (!success)
                    return NotFound();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete story {StoryId}", storyId);
                return StatusCode(500, new { message = "Failed to delete story" });
            }
        }

        [HttpGet("saved")]
        public async Task<ActionResult<IEnumerable<StoryResponseDto>>> GetSavedStories()
        {
            try
            {
                var userId = User.GetUserId();
                var stories = await _storyService.GetSavedStoriesAsync(userId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get saved stories");
                return StatusCode(500, new { message = "Failed to retrieve saved stories" });
            }
        }

        [HttpGet("seen")]
        public async Task<ActionResult<IEnumerable<StoryResponseDto>>> GetSeenStories()
        {
            try
            {
                var userId = User.GetUserId();
                var stories = await _storyService.GetSeenStoriesAsync(userId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get seen stories");
                return StatusCode(500, new { message = "Failed to retrieve seen stories" });
            }
        }

        [HttpPost("{storyId:guid}/toggle-bookmark")]
        public async Task<IActionResult> ToggleBookmark(Guid storyId)
        {
            try
            {
                var userId = User.GetUserId();
                var success = await _storyService.ToggleBookmarkAsync(userId, storyId);
                
                if (success)
                    return Ok();
                
                return BadRequest(new { message = "Failed to toggle bookmark" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to toggle bookmark for story {StoryId}", storyId);
                return StatusCode(500, new { message = "Failed to toggle bookmark" });
            }
        }
    }
}