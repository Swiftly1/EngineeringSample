﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    #nullable enable
    public class Result<T>
    {
        public Result(List<Message> s)
        {
            Success = false;
            Messages = s;
            Message = string.Join(Environment.NewLine, s.Select(x => x.ToString()));
            Data = default;
        }

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
                if (this.Message.Any())
                    return new Result<U>(this.Messages);

                return new Result<U>(this.Message);
            }
        }
    }
}