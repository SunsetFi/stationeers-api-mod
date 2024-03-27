namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a forbidden operation is attempted.
    /// </summary>
    public class ForbiddenException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with the default "Forbidden." message.
        /// </summary>
        public ForbiddenException()
            : base(HttpStatusCode.Forbidden, "Forbidden.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ForbiddenException(string message)
            : base(HttpStatusCode.Forbidden, message)
        {
        }
    }
}
