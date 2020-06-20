
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class ForbiddenException : WebException
    {
        public ForbiddenException() : base(HttpStatusCode.Forbidden, "Forbidden.") { }

        public ForbiddenException(string message) : base(HttpStatusCode.Forbidden, message) { }
    }
}