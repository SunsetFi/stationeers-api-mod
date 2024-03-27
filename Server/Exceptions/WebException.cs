namespace StationeersWebApi.Server.Exceptions
{
    using System;

    /// <summary>
    /// Represents exceptions that are thrown in response to HTTP web requests.
    /// </summary>
    public class WebException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class with the specified status code and message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code associated with this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public WebException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Gets or sets the status code associated with this exception.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}
