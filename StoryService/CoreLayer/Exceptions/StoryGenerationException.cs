namespace CoreLayer.Exceptions
{
    public class StoryGenerationException : Exception
    {
        public StoryGenerationException(string message) : base(message) { }
        public StoryGenerationException(string message, Exception innerException) : base(message, innerException) { }
    }
}