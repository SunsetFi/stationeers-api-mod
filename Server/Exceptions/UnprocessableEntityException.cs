namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the server understands the content type of the request entity,
    /// but is unable to process the contained instructions.
    /// </summary>
    public class UnprocessableEntityException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class with the default "Unprocessable Entity." message.
        /// </summary>
        public UnprocessableEntityException()
            : base(HttpStatusCode.UnprocessableEntity, "Unprocessable Entity.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnprocessableEntityException(string message)
            : base(HttpStatusCode.UnprocessableEntity, message)
        {
        }
    }
}
