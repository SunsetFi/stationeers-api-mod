
using Ceen;

namespace WebAPI.Server
{
    public class NotFoundException : WebException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound, "Not Found") { }

        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
    }
}