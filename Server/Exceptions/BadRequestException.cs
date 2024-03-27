namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Exception for a bad request.
    /// </summary>
    public class BadRequestException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        public BadRequestException()
            : base(HttpStatusCode.BadRequest, "Bad Request.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
