using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusCodeException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }
    }

    public class BadRequestException : HttpStatusCodeException
    {
        public BadRequestException() : base(400)
        {

        }
        public BadRequestException(string message)
            : base(400, message)
        {
        }
    }
}
