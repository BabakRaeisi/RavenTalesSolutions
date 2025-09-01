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
                var preferences = await _userPreferencesService.GetUserPreferencesAsync(userId);
                
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
    }
}