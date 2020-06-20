
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class NotFoundException : WebException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound, "Not Found.") { }

        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
    }
}