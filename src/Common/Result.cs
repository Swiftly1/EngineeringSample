using System;
using System.Collections.Generic;

namespace Common
{
#nullable enable
    public class Result<T>
    {
        public Result(List<Message> s)
        {
            Success = false;
            Messages = s;
            Data = default;
        }

        public Result(string s, DiagnosticInfo d)
        {
            Success = false;
            Messages.Add(Message.CreateError(s, d));
            Data = default;
        }

        public Result(T? data)
        {
            Data = data;
            Success = true;
        }

        public bool Success { get; }

        public Message Message => Messages[0];

        public List<Message> Messages { get; } = new List<Message>();

        public T? Data { get; }

        public Result<U> ToFailedResult<U>()
        {
            if (this.Success)
            {
                throw new Exception("This result was successful, so it shouldn't be changed into failed result.");
            }
            else
            {
                return new Result<U>(this.Messages);
            }
        }
    }
}