using CoreLayer.Enums;
using CoreLayer.ExternalServiceContracts;
using CoreLayer.Constants;
using CoreLayer.Entities;
using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryGenerationMicroservice.API.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIService> _logger;
        private const string SystemPrompt = "You are a helpful assistant that generates language learning stories based on the user's specified language level and topic. Ensure the stories are engaging, appropriate for the specified level, and include vocabulary and grammar suitable for learners.";


        public OpenAIService(IConfiguration configuration, ILogger<OpenAIService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("OpenAI API key is not configured");
                
            var model = _configuration["OpenAI:Model"] ?? OpenAIConstants.DefaultModel;
            _chatClient = new ChatClient(model, apiKey);
        }

        public async Task<Story> GenerateStoryAsync(LanguageLevel level, TargetLanguage language, string? topic = null)
        {
            try
            {
                var prompt = BuildPrompt(level, language, topic);

                var maxTokens = _configuration.GetValue<int?>("OpenAI:MaxTokens") ?? OpenAIConstants.MaxTokens;
                var temperature = _configuration.GetValue<float?>("OpenAI:Temperature") ?? (float)OpenAIConstants.Temperature;

                var response = await _chatClient.CompleteChatAsync(
                    [
                        new SystemChatMessage(SystemPrompt),
                        new UserChatMessage(prompt)
                    ],
                    new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    });

                _logger.LogInformation("OpenAI response: {Response}", response.Value.Content[0].Text);
                var json = response.Value.Content[0].Text;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                _logger.LogInformation("OpenAI raw JSON response: {Json}", json);
                return JsonSerializer.Deserialize<Story>(json, options)
                    ?? throw new InvalidOperationException("Failed to parse story JSON.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate story for {Level} level in {Language}. Topic: {Topic}", 
                    level, language, topic ?? "None");
                throw new InvalidOperationException("Story generation failed", ex);
            }
        }

        private string BuildPrompt(LanguageLevel level, TargetLanguage language, string? topic)
        {
            var levelDescription = OpenAIConstants.LevelDescriptions.GetValueOrDefault(level, level.ToString());
            var topicPart = string.IsNullOrEmpty(topic) ? "" : $" about {topic}";

            return $@"Create a story in {language}{topicPart}.

Requirements:
- Return ONLY a valid JSON object matching the following C# class:
  public class Story {{
    public string Title {{ get; set; }};
    public string Content {{ get; set; }};
    public string Genre {{ get; set; }};
    public string LanguageLevel {{ get; set; }};
    public string TargetLanguage {{ get; set; }};
  }}
- For 'LanguageLevel', use one of: A1, A2, B1, B2, C1, C2 (case-sensitive, no extra text).
- For 'TargetLanguage', use one of: English, Spanish, French, German, Italian, etc. (case-sensitive, no extra text).
- Do not include any explanations or text outside the JSON.
- The 'Title' should be a short, engaging name for the story.
- The 'Content' should be the full story text.
- The 'Genre' should be a single word describing the story type (e.g., Adventure, Mystery).
- 'LanguageLevel' should be ""{level}"".
- 'TargetLanguage' should be ""{language}"".
- The story should be approximately {GetWordCount(level)} words long.

Example format:
{{
  ""Title"": ""A Day at the Market"",
  ""Content"": ""Once upon a time..."",
  ""Genre"": ""Adventure"",
  ""LanguageLevel"": ""{level}"",
  ""TargetLanguage"": ""{language}""
}}";
        }

        private int GetWordCount(LanguageLevel level) => level switch
        {
            LanguageLevel.A1 => 100,
            LanguageLevel.A2 => 150,
            LanguageLevel.B1 => 200,
            LanguageLevel.B2 => 300,
            LanguageLevel.C1 => 400,
            LanguageLevel.C2 => 500,
            _ => 200
        };
    }

 
}