#nullable enable
namespace Common
{
    public class Result<T>
    {
        private Result() { }

        public bool Success { get; private set; }

        public string? Message { get; private set; }

        public T? Data { get; private set; }

        public static Result<T> Error(string error)
        {
            return new Result<T>()
            {
                Success = false,
                Message = error,
                Data = default(T)
            };
        }

        public static Result<T> Ok(T data)
        {
            return new Result<T>()
            {
                Success = true,
                Message = "",
                Data = data
            };
        }
    }
}

