namespace StationeersWebApi.Server
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements a IWebRouteHttpContext based on an IHttpContext.
    /// </summary>
    public class WebRouteHttpContext : IWebRouteHttpContext
    {
        private IHttpContext httpContext;
        private IDictionary<string, string> pathParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRouteHttpContext"/> class.
        /// </summary>
        /// <param name="httpContext">The backing IHttpContext.</param>
        /// <param name="pathParameters">The path parameters for this context.</param>
        public WebRouteHttpContext(IHttpContext httpContext, IDictionary<string, string> pathParameters)
        {
            this.httpContext = httpContext;
            this.pathParameters = pathParameters;
        }

        /// <inheritdoc/>
        public IPEndPoint RemoteEndPoint => this.httpContext.RemoteEndPoint;


        /// <inheritdoc/>
        public string Method => this.httpContext.Method;

        /// <inheritdoc/>
        public string Path => this.httpContext.Path;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> QueryString => this.httpContext.QueryString;

        /// <inheritdoc/>
        public IDictionary<string, string> PathParameters
        {
            get
            {
                return this.pathParameters;
            }

            set
            {
                this.pathParameters = value;
            }
        }

        public IReadOnlyDictionary<string, string> Headers => this.httpContext.Headers;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> Cookies => this.httpContext.Cookies;

        /// <inheritdoc/>
        public Stream Body => this.httpContext.Body;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.httpContext.Dispose();
        }

        /// <inheritdoc/>
        public void SetResponseHeader(string header, string value)
        {
            this.httpContext.SetResponseHeader(header, value);
        }

        /// <inheritdoc/>
        public void AddResponseCookie(Cookie cookie)
        {
            this.httpContext.AddResponseCookie(cookie);
        }

        /// <inheritdoc/>
        public Task SendResponse(HttpStatusCode statusCode)
        {
            return this.httpContext.SendResponse(statusCode);
        }

        /// <inheritdoc/>
        public Task SendResponse(HttpStatusCode statusCode, string contentType, Stream response)
        {
            return this.httpContext.SendResponse(statusCode, contentType, response);
        }
    }
}
