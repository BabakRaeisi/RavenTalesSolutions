// CoreLayer/Exceptions/PreferencesMissingException.cs
namespace CoreLayer.Exceptions
{
    public sealed class PreferencesMissingException : Exception
    {
        public PreferencesMissingException(Guid userId)
            : base($"User preferences are required for user {userId} but were not found.") { }
    }
}