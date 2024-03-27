namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Exception for an internal server error.
    /// </summary>
    public class InternalServerErrorException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        public InternalServerErrorException()
            : base(HttpStatusCode.InternalServerError, "Internal Server Error.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InternalServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }
    }
}
