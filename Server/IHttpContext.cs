namespace StationeersWebApi.Server
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Http Context for a StationeersWebApi Route request.
    /// </summary>
    public interface IHttpContext : IDisposable
    {
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets the request method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the request path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        IReadOnlyDictionary<string, string> QueryString { get; }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        IReadOnlyDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        IReadOnlyDictionary<string, string> Cookies { get; }

        /// <summary>
        /// Gets the body stream.
        /// </summary>
        Stream Body { get; }

        /// <summary>
        /// Sets a header on the response.
        /// </summary>
        /// <param name="header">The header name.</param>
        /// <param name="value">The header value.</param>
        void SetResponseHeader(string header, string value);

        /// <summary>
        /// Sets a cookie on the response.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        void AddResponseCookie(Cookie cookie);

        /// <summary>
        /// Sends a response to the quest.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendResponse(HttpStatusCode statusCode);

        /// <summary>
        /// Sends a response to the request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="response">The response body.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendResponse(HttpStatusCode statusCode, string contentType, Stream response);
    }
}
