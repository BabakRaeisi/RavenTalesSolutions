using CoreLayer.DTOs;
using CoreLayer.ServiceContracts;
using CoreLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryGenerationMicroservice.API.Extensions;

namespace StoryGenerationMicroservice.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserPreferencesController : ControllerBase
    {
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly ILogger<UserPreferencesController> _logger;

        public UserPreferencesController(IUserPreferencesService userPreferencesService, ILogger<UserPreferencesController> logger)
        {
            _userPreferencesService = userPreferencesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<UserPreferencesResponseDto>> GetUserPreferences()
        {
            try
            {
                var userId = User.GetUserId();
                var preferences = await _userPreferencesService.GetUserByIdAsync(userId);
                
                if (preferences == null)
                    return NotFound();
                
                return Ok(preferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user preferences");
                return StatusCode(500, new { message = "Failed to retrieve preferences" });
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserPreferencesResponseDto>> UpdateUserPreferences([FromBody] UserPreferencesUpdateRequestDto request)
        {
            try
            {
                var userId = User.GetUserId();
                var preferences = await _userPreferencesService.UpdateUserPreferencesAsync(userId, request);
                return Ok(preferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user preferences");
                return StatusCode(500, new { message = "Failed to update preferences" });
            }
        }


      
        [HttpPost("SaveStory")]
        public async Task<ActionResult<StoryResponseDto>> SaveStory([FromBody] StoryResponseDto request)
        {
            try
            {
                var storyId = request.Id;
                var userId = User.GetUserId();
                await _userPreferencesService.AddToUserSavedStoriesAsync(userId, storyId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save story");
                return StatusCode(500, new { message = "Failed to save story" });
            }
        }
        [HttpGet("saved")]
        public async Task<ActionResult<IEnumerable<StoryResponseDto>>> GetUserStories()
        {
            try
            {
                var userId = User.GetUserId();
                var stories = await _userPreferencesService.GetUserSavedStoriesAsync(userId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user stories");
                return StatusCode(500, new { message = "Failed to retrieve stories" });
            }
        }


        [HttpGet("seen")]
        public async Task<ActionResult<IEnumerable<StoryResponseDto>>> GetSeenStories()
        {
            try
            {
                var userId = User.GetUserId();
                var stories = await _userPreferencesService.GetUserStoryHistoryAsync(userId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get seen stories");
                return StatusCode(500, new { message = "Failed to retrieve seen stories" });
            }
        }
        [HttpPost("{storyId:guid}/bookmark")]
        public async Task<IActionResult> ToggleBookmark(Guid storyId)
        {
            var userId = User.GetUserId();
            await _userPreferencesService.AddToUserSavedStoriesAsync(userId, storyId);
            return NoContent(); // or return Ok();
        }

    }
}