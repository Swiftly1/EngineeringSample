namespace Common
{
    public class Result
    {
        public Result() { }

        public Result(string s)
        {
            Success = false;
            Message = s;
        }

        public readonly bool Success = true;

        public readonly string Message;
    }
}