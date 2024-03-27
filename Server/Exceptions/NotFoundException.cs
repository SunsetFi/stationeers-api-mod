namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a requested resource is not found.
    /// </summary>
    public class NotFoundException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with the default "Not Found." message.
        /// </summary>
        public NotFoundException()
            : base(HttpStatusCode.NotFound, "Not Found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
