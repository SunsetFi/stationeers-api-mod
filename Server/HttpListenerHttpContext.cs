namespace StationeersWebApi.Server
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using HarmonyLib;

    /// <summary>
    /// Provides an HTTP Context from an HttpListener context.
    /// </summary>
    public class HttpListenerHttpContext : IHttpContext
    {
        private readonly HttpListenerContext context;
        private readonly IReadOnlyDictionary<string, string> queryString;

        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerHttpContext"/> class.
        /// </summary>
        /// <param name="context">The context to wrap.</param>
        public HttpListenerHttpContext(HttpListenerContext context)
        {
            this.context = context;
            this.queryString = context.Request.QueryString.AllKeys.Where(x => x != null).ToDictionary(key => key, key => context.Request.QueryString[key]);
        }

        /// <inheritdoc/>
        public IPEndPoint RemoteEndPoint => this.context.Request.RemoteEndPoint;

        /// <inheritdoc/>
        public string Method => this.context.Request.HttpMethod;

        /// <inheritdoc/>
        public string Path => this.context.Request.Url.LocalPath;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> QueryString => this.queryString;

        /// <inheritdoc/>
        public Stream Body => this.context.Request.InputStream;


        private IReadOnlyDictionary<string, string> headers;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> Headers
        {
            get
            {
                if (this.headers == null)
                {
                    var headers = new Dictionary<string, string>();
                    foreach (var key in this.context.Request.Headers.AllKeys)
                    {
                        headers.Add(key, this.context.Request.Headers[key]);
                    }
                    this.headers = headers;
                }

                return this.headers;
            }
        }

        private IReadOnlyDictionary<string, string> cookies;
        public IReadOnlyDictionary<string, string> Cookies
        {
            get
            {
                if (this.cookies == null)
                {
                    var cookies = new Dictionary<string, string>();
                    foreach (var cookie in this.context.Request.Cookies.Cast<Cookie>())
                    {
                        cookies.Add(cookie.Name, this.context.Request.Cookies[cookie.Name].Value);
                    }
                    this.cookies = cookies;
                }

                return this.cookies;
            }
        }

        /// <inheritdoc/>
        public void SetResponseHeader(string header, string value)
        {
            this.context.Response.Headers.Add(header, value);
        }

        /// <inheritdoc/>
        public void AddResponseCookie(Cookie cookie)
        {
            this.context.Response.Cookies.Add(cookie);
        }

        /// <inheritdoc/>
        public Task SendResponse(HttpStatusCode statusCode)
        {
            this.context.Response.StatusCode = (int)statusCode;
            this.context.Response.Headers.Add("Content-Length", "0");

            this.Dispose();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SendResponse(HttpStatusCode statusCode, string contentType, Stream response)
        {
            this.context.Response.StatusCode = (int)statusCode;
            this.context.Response.ContentType = contentType;

            response.CopyTo(this.context.Response.OutputStream);

            this.Dispose();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.context.Response.Close();
                this.isDisposed = true;
            }
        }
    }
}
