using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    /// <summary>
    /// 表示带有HTTP状态码的异常。
    /// </summary>
    public class HttpStatusCodeException : Exception
    {
        /// <summary>
        /// 获取或设置HTTP状态码。
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 获取或设置响应内容类型。
        /// </summary>
        public string ContentType { get; set; } = @"text/plain";

        /// <summary>
        /// 初始化 <see cref="HttpStatusCodeException"/> 类的新实例。
        /// </summary>
        /// <param name="statusCode">HTTP状态码。</param>
        public HttpStatusCodeException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// 初始化 <see cref="HttpStatusCodeException"/> 类的新实例。
        /// </summary>
        /// <param name="statusCode">HTTP状态码。</param>
        /// <param name="message">异常消息。</param>
        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// 初始化 <see cref="HttpStatusCodeException"/> 类的新实例。
        /// </summary>
        /// <param name="statusCode">HTTP状态码。</param>
        /// <param name="inner">内部异常。</param>
        public HttpStatusCodeException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }
    }

    /// <summary>
    /// 表示HTTP 400 Bad Request异常。
    /// </summary>
    public class BadRequestException : HttpStatusCodeException
    {
        /// <summary>
        /// 初始化 <see cref="BadRequestException"/> 类的新实例。
        /// </summary>
        public BadRequestException() : base(400)
        {

        }

        /// <summary>
        /// 初始化 <see cref="BadRequestException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public BadRequestException(string message)
            : base(400, message)
        {
        }
    }
}
