using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Howl.Core
{
    /// <summary>
    /// 表示操作结果的基类，用于封装操作的状态、消息和错误信息。
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 结果桥接类，用于在不同类型的Result之间转换。
        /// </summary>
        public class Bridge : Result
        {
            //public static implicit operator Task<Result>(Bridge input) => Task.FromResult((Result)input);
        }

        /// <summary>
        /// 获取或设置操作是否成功。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取或设置HTTP状态码或自定义状态码。
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 获取或设置内部错误代码。
        /// </summary>
        public string InnerCode { get; set; }

        /// <summary>
        /// 获取或设置操作耗时（毫秒）。
        /// </summary>
        public double? ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 获取或设置结果消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置子结果集合，用于嵌套操作结果。
        /// </summary>
        public Result[] Children { get; set; }

        /// <summary>
        /// 获取一个表示操作成功的静态结果实例。
        /// </summary>
        public static Bridge Succeeded => new Bridge { Success = true, Code = 200 };

        /// <summary>
        /// 获取一个表示操作成功的已完成任务结果。
        /// </summary>
        public static Task<Result> SucceededTask { get; } = Task.FromResult((Result)Succeeded);

        /// <summary>
        /// 将当前结果转换为带数据类型的泛型结果。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="data">要设置的数据值，默认为类型的默认值。</param>
        /// <returns>包含数据的泛型结果实例。</returns>
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

        /// <summary>
        /// 创建一个不包含数据的桥接结果，保留当前结果的所有属性。
        /// </summary>
        /// <returns>新的桥接结果实例。</returns>
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

    /// <summary>
    /// 表示带数据类型的操作结果。
    /// </summary>
    /// <typeparam name="T">结果数据的类型。</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 初始化 <see cref="Result{T}"/> 类的新实例。
        /// </summary>
        public Result()
        {

        }

        /// <summary>
        /// 获取或设置结果数据。
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 将数据隐式转换为成功的结果实例。
        /// </summary>
        /// <param name="input">要转换的数据。</param>
        /// <returns>包含数据的成功结果实例。</returns>
        public static implicit operator Result<T>(T input) => ResultBuilder.Succeed(input);

        /// <summary>
        /// 将桥接结果隐式转换为泛型结果实例。
        /// </summary>
        /// <param name="input">要转换的桥接结果。</param>
        /// <returns>包含桥接结果属性的泛型结果实例。</returns>
        public static implicit operator Result<T>(Bridge input) => new Result<T> { Success = input.Success, Data = default(T), Code = input.Code, InnerCode = input.InnerCode, Message = input.Message };

        /// <summary>
        /// 将泛型结果隐式转换为已完成的任务。
        /// </summary>
        /// <param name="input">要转换的结果实例。</param>
        /// <returns>包含结果的已完成任务。</returns>
        public static implicit operator Task<Result<T>>(Result<T> input) => Task.FromResult(input);
    }
}
