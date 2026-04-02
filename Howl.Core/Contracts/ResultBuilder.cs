using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Howl.Core
{
    /// <summary>
    /// 提供创建 <see cref="Result"/> 和 <see cref="Result{T}"/> 实例的静态工厂方法。
    /// </summary>
    public static class ResultBuilder
    {
        /// <summary>
        /// 创建一个表示操作成功的带数据结果。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="data">结果数据。</param>
        /// <param name="message">可选的结果消息。</param>
        /// <returns>包含数据和成功状态的结果实例。</returns>
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

        /// <summary>
        /// 创建一个表示操作失败的桥接结果，状态码默认为400。
        /// </summary>
        /// <param name="message">可选的错误消息。</param>
        /// <param name="innerStatusCode">可选的内部错误代码。</param>
        /// <returns>表示失败的桥接结果实例。</returns>
        public static Result.Bridge Fail(string message = null, string innerStatusCode = null)
        {
            return Fail(400, message, innerStatusCode);
        }

        /// <summary>
        /// 创建一个表示操作失败的桥接结果，使用指定的状态码。
        /// </summary>
        /// <param name="statusCode">HTTP状态码或自定义状态码。</param>
        /// <param name="message">可选的错误消息。</param>
        /// <param name="innerStatusCode">可选的内部错误代码。</param>
        /// <returns>表示失败的桥接结果实例。</returns>
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

        /// <summary>
        /// 创建一个表示操作失败的带数据结果。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="data">结果数据。</param>
        /// <param name="statusCode">HTTP状态码或自定义状态码。</param>
        /// <param name="message">可选的错误消息。</param>
        /// <param name="innerStatusCode">可选的内部错误代码。</param>
        /// <returns>包含数据和失败状态的结果实例。</returns>
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

        /// <summary>
        /// 创建一个表示参数为null的错误结果。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <returns>状态码为400的桥接结果实例。</returns>
        public static Result.Bridge Null(string name)
        {
            return Fail(400, $"{name} can not be null.");
        }

        /// <summary>
        /// 创建一个表示资源未找到的错误结果。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <returns>状态码为404的桥接结果实例。</returns>
        public static Result.Bridge NotFound(string name)
        {
            return Fail(404, $"{name} can not be found.");
        }

        /// <summary>
        /// 创建一个表示资源已禁用的错误结果。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <returns>状态码为400的桥接结果实例。</returns>
        public static Result.Bridge Disabled(string name)
        {
            return Fail(400, $"{name} is disabled.");
        }

        /// <summary>
        /// 创建一个表示服务器内部错误的桥接结果。
        /// </summary>
        /// <param name="message">可选的错误消息。</param>
        /// <returns>状态码为500的桥接结果实例。</returns>
        public static Result.Bridge Error(string message = null)
        {
            return Fail(500, message);
        }

        /// <summary>
        /// 处理异常并返回表示服务器错误的桥接结果。
        /// 会自动解包 <see cref="AggregateException"/> 以获取内部异常。
        /// </summary>
        /// <param name="exception">要处理的异常实例。</param>
        /// <returns>状态码为500的桥接结果实例。</returns>
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
