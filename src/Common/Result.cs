namespace Common
{
    #nullable enable
    public class Result<T>
    {
        public Result(string s)
        {
            Success = false;
            Message = s;
            Data = default;
        }

        public Result(T? data)
        {
            Data = data;
            Success = true;
            Message = "";
        }

        public bool Success { get; }

        public string Message { get; }

        public T? Data { get; }
    }
}