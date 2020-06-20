
using Ceen;

namespace WebAPI.Server.Exceptions
{
    public class UnprocessableEntityException : WebException
    {
        public UnprocessableEntityException() : base(HttpStatusCode.UnprocessableEntity, "Unprocessable Entity.") { }
        public UnprocessableEntityException(string message) : base(HttpStatusCode.UnprocessableEntity, message) { }
    }
}