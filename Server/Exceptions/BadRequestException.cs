
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class BadRequestException : WebException
    {
        public BadRequestException() : base(HttpStatusCode.NotFound, "Bad Request.") { }

        public BadRequestException(string message) : base(HttpStatusCode.NotFound, message) { }
    }
}