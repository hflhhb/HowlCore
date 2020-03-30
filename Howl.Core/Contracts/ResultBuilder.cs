using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Howl.Core
{
    public static class ResultBuilder
    {
        public static Result<T> Succeed<T>(T data, string message = null)
        {
            return new Result<T>
            {
                Data = data,
                Message = message,
                Code = 200,
                Success = true
            };
        }
        public static Result.Bridge Fail(string message = null, string innerStatusCode = null)
        {
            return Fail(400, message, innerStatusCode);
        }
        public static Result.Bridge Fail(int statusCode, string message = null, string innerStatusCode = null)
        {
            return new Result.Bridge
            {
                Success = false,
                Code = statusCode,
                Message = message,
                InnerCode = innerStatusCode
            };
        }
        public static Result<T> Fail<T>(T data, int statusCode, string message = null, string innerStatusCode = null)
        {
            return new Result<T>
            {
                Success = false,
                Code = statusCode,
                Message = message,
                Data = data,
                InnerCode = innerStatusCode
            };
        }

        public static Result.Bridge Null(string name)
        {
            return Fail(400, $"{name} can not be null.");
        }
        public static Result.Bridge NotFound(string name)
        {
            return Fail(404, $"{name} can not be found.");
        }
        public static Result.Bridge Disabled(string name)
        {
            return Fail(400, $"{name} is disabled.");
        }
        public static Result.Bridge Error(string message = null)
        {
            return Fail(500, message);
        }
        public static Result.Bridge Handle(Exception exception)
        {
            if (exception != null)
            {
                while (exception is AggregateException aggregateException)
                    exception = aggregateException.InnerException;

                //
                Debug.WriteLine(exception);
            }

            return Fail(500);
        }
    }
}
