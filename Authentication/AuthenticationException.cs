
using System;
using Ceen;

namespace WebAPI.Authentication
{
    public class AuthenticationException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public AuthenticationException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}