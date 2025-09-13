namespace StoryGenerationMicroservice.API.Middleware
{
    using System.Text.Json;
    using CoreLayer.Exceptions;        // <-- your Core exceptions live here

    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PreferencesMissingException)
            {
                await WriteJson(context, 428, "PREFS_MISSING",
                    "User preferences are required before this action.",
                    "/onboarding/preferences");
            }
            catch (StoryGenerationException)
            {
                await WriteJson(context, StatusCodes.Status502BadGateway, "STORY_GENERATION_FAILED",
                    "Story generation failed. Please try again.",
                    null);
            }
        }

        private static Task WriteJson(
            HttpContext context, int status, string code, string message, string? setupPath)
        {
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";
            var payload = new { errorCode = code, message, setupPath };
            return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}