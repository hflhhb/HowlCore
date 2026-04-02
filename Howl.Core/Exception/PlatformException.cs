using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    /// <summary>
    /// 表示平台级别的异常，包含错误代码和附加数据。
    /// </summary>
    public class PlatformException : Exception
    {
        /// <summary>
        /// 获取错误代码。
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// 获取附加数据。
        /// </summary>
        public object ExData { get; }

        /// <summary>
        /// 获取异常消息。
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// 初始化 <see cref="PlatformException"/> 类的新实例，默认错误代码为400。
        /// </summary>
        public PlatformException() : base()
        {
            Code = 400;
        }

        /// <summary>
        /// 初始化 <see cref="PlatformException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public PlatformException(string message) : this()
        {
            Message = message;
        }

        /// <summary>
        /// 初始化 <see cref="PlatformException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="data">附加数据。</param>
        /// <param name="code">错误代码，默认为400。</param>
        public PlatformException(string message, object data = null, int code = 400) : this(message)
        {
            Code = code;
            ExData = data;
        }

        /// <summary>
        /// 初始化 <see cref="PlatformException"/> 类的新实例。
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">异常消息。</param>
        /// <param name="data">附加数据。</param>
        public PlatformException(int code, string message = null, object data = null) : this(message)
        {
            Code = code;
            ExData = data;
        }

    }
}
