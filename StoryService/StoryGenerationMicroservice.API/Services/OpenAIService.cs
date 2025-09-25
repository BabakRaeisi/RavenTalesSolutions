
using CoreLayer.ExternalServiceContracts;
using CoreLayer.Constants;
using CoreLayer.Entities;
using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;
using RavenTales.Shared;

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

        public async Task<Story> GenerateStoryAsync(LanguageLevel level,  Language language, string? topic = null)
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
                        Temperature = temperature,
                        ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat() // enforce pure JSON
                    });

                var json = response.Value.Content[0].Text;
                _logger.LogInformation("OpenAI raw JSON response: {Json}", json);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var story = JsonSerializer.Deserialize<Story>(json, options)
                    ?? throw new InvalidOperationException("Failed to parse story JSON.");

                // Ensure enums match the request exactly (model sometimes drifts)
                story.LanguageLevel = level;
                story.StoryLanguage = language;

                return story;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate story for {Level} level in {Language}. Topic: {Topic}",
                    level, language, topic ?? "None");
                throw new InvalidOperationException("Story generation failed", ex);
            }
        }

        private string BuildPrompt(LanguageLevel level, Language language, string? topic)
        {
            var levelDescription = OpenAIConstants.LevelDescriptions.GetValueOrDefault(level, level.ToString());
            var topicPart = string.IsNullOrEmpty(topic) ? "" : $" about {topic}";

            return $@"Create a story in {language}{topicPart}.

Output rules (very important):
- Return ONLY a single valid JSON object. No prose, no markdown fences.
- Use standard JSON (double quotes, no trailing commas).
- The JSON must match exactly this shape (keys and types):
  {{
    ""Title"": string,
    ""Content"": string,
    ""Genre"": string,
    ""LanguageLevel"": string,   // one of: A1, A2, B1, B2, C1, C2 (case-sensitive, no extra text)
    ""StoryLanguage"": string,  // e.g., English, Spanish, French, German, Italian, etc. (case-sensitive, no extra text).
    ""Sentences"": [
      {{ ""Id"": string, ""StartChar"": number, ""EndChar"": number, ""Text"": string }}
    ],
    ""Units"": [
      {{
        ""Id"": string,
        ""SentenceId"": string,
        ""IsDiscontinuous"": boolean,
        ""Segments"": [ {{ ""StartChar"": number, ""EndChar"": number }} ],
        ""Pieces"": [ string ]
      }}
    ]
  }}

Story requirements:
- ""LanguageLevel"" must be ""{level}"" (case-sensitive).
- ""StoryLanguage"" must be ""{language}"" (case-sensitive).
- ""Title"" is short and engaging.
- ""Genre"" is a single word (e.g., Adventure, Mystery).
- ""Content"" is the full story text (about {GetWordCount(level)} words), with normal spacing and punctuation.

Sentence segmentation:
- Split ""Content"" into sentences. For each sentence, include one object in ""Sentences"".
- ""StartChar"" is 0-based, inclusive; ""EndChar"" is 0-based, exclusive; both index into the full ""Content"".
- ""Text"" must exactly equal Content[StartChar:EndChar].

Units (the clickable �little parts�):
- Each Unit represents words that function together as one idea; Units may be continuous or discontinuous in the sentence.
- For every Unit:
  - ""SentenceId"" points to the sentence it lives in.
  - ""Segments"" lists 1..N character ranges (0-based inclusive/exclusive) into the full ""Content"".
  - ""Pieces"" lists the exact substrings for each segment, in order. Each Pieces[k] must equal Content[Segments[k].StartChar : Segments[k].EndChar].
  - ""IsDiscontinuous"" is true if there are 2+ segments; otherwise false.
- Do NOT include any extra keys (no ""Type"", no ""Label"", no translations, no tokens, no POS).

Minimality rules (avoid redundancy):
- Do NOT create standalone verb-only units or loose collocations.
- Only include the core, meaningful groupings:
  � French:
    1) the negation particle pair (e.g., ""ne � pas/plus/jamais/rien/personne/que"") as one Unit; and
    2) the combined subject + negation + finite verb + negative particle as one Unit (e.g., ""Je � ne � VERB � pas""), when present.
  � Spanish:
    1) the negated finite verb as one Unit combining ""No"" + the finite verb even if words appear between them (e.g., ""No � puedo""); and
    2) any object clitic + infinitive pairing as one Unit (e.g., ""lo � explicar"").
- Do NOT reorder or alter words. ""Pieces"" must be exact surface text as it appears in ""Content"".
- Do NOT emit extra units that are subsets of other units (e.g., the finite verb alone if a negated unit already covers it).

Validation checks before you output:
- Every ""Text"" equals the ""Content"" slice defined by its StartChar/EndChar.
- Every ""Pieces"" item equals its corresponding ""Content"" slice.
- All indices are within bounds of ""Content"".

Return only the final JSON object that follows these rules.";
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