using CoreLayer.Enums;

namespace CoreLayer.Constants
{
    public static class OpenAIConstants
    {
        public const string DefaultModel = "gpt-4o-mini";
        public const int MaxTokens = 1000;
        public const decimal Temperature = 0.7m;
      
        public static readonly Dictionary<LanguageLevel, string> LevelDescriptions = new()
        {
            { LanguageLevel.A1, "Beginner level with simple vocabulary and basic grammar" },
            { LanguageLevel.A2, "Elementary level with everyday expressions" },
            { LanguageLevel.B1, "Intermediate level with complex ideas" },
            { LanguageLevel.B2, "Upper intermediate with abstract topics" },
            { LanguageLevel.C1, "Advanced level with sophisticated language" },
            { LanguageLevel.C2, "Proficient level with native-like fluency" }
        };
    }
}