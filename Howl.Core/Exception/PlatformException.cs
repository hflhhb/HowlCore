using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    public class PlatformException : Exception
    {
        /// <summary>
        /// error code
        /// </summary>
        public int Code { get; private set; }
        /// <summary>
        /// Extra Data
        /// </summary>
        public object ExData { get; }

        public override string Message { get; }

        public PlatformException() : base()
        {
            Code = 400;
        }

        public PlatformException(string message) : this()
        {
            Message = message;
        }

        public PlatformException(string message, object data = null, int code = 400) : this(message)
        {
            Code = code;
            ExData = data;
        }

        public PlatformException(int code, string message = null, object data = null) : this(message)
        {
            Code = code;
            ExData = data;
        }

    }
}
