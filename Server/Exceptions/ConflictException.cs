namespace StationeersWebApi.Server.Exceptions
{
    /// <summary>
    /// Exception for a conflict.
    /// </summary>
    public class ConflictException : WebException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        public ConflictException()
            : base(HttpStatusCode.Conflict, "Conflict.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
