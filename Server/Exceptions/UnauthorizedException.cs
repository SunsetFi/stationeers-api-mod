namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an unauthorized operation is attempted.
    /// </summary>
    public class UnauthorizedException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with the default "Unauthorized." message.
        /// </summary>
        public UnauthorizedException()
            : base(HttpStatusCode.Unauthorized, "Unauthorized.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
