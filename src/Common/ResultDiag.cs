using System;
using System.Collections.Generic;

namespace Common
{
#nullable enable
    public class ResultDiag<T>
    {
        public ResultDiag(List<Message> s)
        {
            Success = false;
            Messages = s;
            Data = default;
        }

        public ResultDiag(Message m)
        {
            Success = false;
            Messages.Add(m);
            Data = default;
        }

        public ResultDiag(string s, DiagnosticInfo d)
        {
            Success = false;
            Messages.Add(Message.CreateError(s, d));
            Data = default;
        }

        public ResultDiag(T? data)
        {
            Data = data;
            Success = true;
        }

        public bool Success { get; }

        public List<Message> Messages { get; } = new List<Message>();

        public T? Data { get; }

        public ResultDiag<U> ToFailedResult<U>()
        {
            if (this.Success)
            {
                throw new Exception("This result was successful, so it shouldn't be changed into failed result.");
            }
            else
            {
                return new ResultDiag<U>(this.Messages);
            }
        }
    }
}