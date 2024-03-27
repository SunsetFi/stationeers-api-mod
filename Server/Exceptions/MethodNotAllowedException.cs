namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a method is not allowed.
    /// </summary>
    public class MethodNotAllowedException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotAllowedException"/> class with the default "Method Not Allowed." message.
        /// </summary>
        public MethodNotAllowedException()
            : base(HttpStatusCode.MethodNotAllowed, "Method Not Allowed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotAllowedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MethodNotAllowedException(string message)
            : base(HttpStatusCode.MethodNotAllowed, message)
        {
        }
    }
}
