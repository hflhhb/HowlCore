using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Howl.Core
{
    public class Result
    {
        public class Bridge : Result
        {
            //public static implicit operator Task<Result>(Bridge input) => Task.FromResult((Result)input);
        }
        public bool Success { get; set; }
        public int? Code { get; set; }
        public string InnerCode { get; set; }
        public double? ElapsedMilliseconds { get; set; }
        public string Message { get; set; }
        public Result[] Children { get; set; }
        public static Bridge Succeeded => new Bridge { Success = true, Code = 200 };
        public static Task<Result> SucceededTask { get; } = Task.FromResult((Result)Succeeded);

        public Result<T> Datalize<T>(T data = default(T))
        {
            var typed = this as Result<T>;

            return typed ?? new Result<T>
            {
                Children = Children,
                Data = data,
                ElapsedMilliseconds = ElapsedMilliseconds,
                Message = Message,
                Success = Success,
                Code = Code,
                InnerCode = InnerCode
            };
        }
        public Bridge Erase()
        {
            //return this.Map(new Bridge());
            return new Bridge
            {
                Children = Children,
                ElapsedMilliseconds = ElapsedMilliseconds,
                Message = Message,
                Success = Success,
                Code = Code,
                InnerCode = InnerCode
            };
        }
    }
    public class Result<T> : Result
    {
        public Result()
        {

        }

        public T Data { get; set; }

        public static implicit operator Result<T>(T input) => ResultBuilder.Succeed(input);
        public static implicit operator Result<T>(Bridge input) => new Result<T> { Success = input.Success, Data = default(T), Code = input.Code, InnerCode = input.InnerCode, Message = input.Message };
        public static implicit operator Task<Result<T>>(Result<T> input) => Task.FromResult(input);
    }
}
