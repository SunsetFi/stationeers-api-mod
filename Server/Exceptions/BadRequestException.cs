
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class BadRequestException : WebException
    {
        public BadRequestException() : base(HttpStatusCode.BadRequest, "Bad Request.") { }

        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }
    }
}