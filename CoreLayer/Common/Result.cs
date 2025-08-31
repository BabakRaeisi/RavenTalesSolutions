namespace CoreLayer.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public string? ErrorMessage { get; init; }
        public List<string> ValidationErrors { get; init; } = new();

        public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static Result<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
        public static Result<T> ValidationFailure(List<string> errors) => new() { IsSuccess = false, ValidationErrors = errors };
    }
}