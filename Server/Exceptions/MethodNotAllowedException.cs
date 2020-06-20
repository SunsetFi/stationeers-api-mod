
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class MethodNotAllowedException : WebException
    {
        public MethodNotAllowedException() : base(HttpStatusCode.MethodNotAllowed, "Method Not Allowed.") { }

        public MethodNotAllowedException(string message) : base(HttpStatusCode.MethodNotAllowed, message) { }
    }
}