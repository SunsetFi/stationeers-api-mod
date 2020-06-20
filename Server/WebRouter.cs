
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Server
{
    public class WebRouter
    {
        private List<IWebRoute> _routes = new List<IWebRoute>();

        public void AddRoute(IWebRoute route)
        {
            _routes.Add(route);
        }

        public int Count
        {
            get
            {
                return this._routes.Count;
            }
        }

        public async Task<bool> HandleRequest(IHttpContext context)
        {
            var request = context.Request;

            foreach (var route in _routes)
            {
                if (request.Method != route.Method)
                {
                    continue;
                }

                var pathParameters = MatchRoute(request.Path, route.Path);
                if (pathParameters == null)
                {
                    continue;
                }


                var routeContext = new WebRouteContext(context, pathParameters);
                await route.OnRequested(routeContext);
                return true;
            }

            return false;
        }

        IDictionary<string, string> MatchRoute(string requestPath, string routePath)
        {
            var pathSegments = requestPath.Split('/');
            var segments = routePath.Split('/');

            if (pathSegments.Length - 1 != segments.Length)
            {
                return null;
            }

            var pathParams = new Dictionary<string, string>();

            for (var i = 0; i < segments.Length; i++)
            {
                var pathSegment = pathSegments[i + 1];
                var matchSegment = segments[i];

                if (matchSegment.StartsWith(":"))
                {
                    pathParams.Add(matchSegment.Substring(1), pathSegment);
                }
                else if (pathSegment.ToLower() != matchSegment.ToLower())
                {
                    return null;
                }
            }

            return pathParams;
        }
    }

    class WebRouteContext : IWebRouteContext
    {
        private IHttpContext httpContext;
        private IDictionary<string, string> pathParameters;

        public WebRouteContext(IHttpContext httpContext, IDictionary<string, string> pathParameters)
        {
            this.httpContext = httpContext;
            this.pathParameters = pathParameters;
        }

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

        public IHttpRequest Request => this.httpContext.Request;

        public IHttpResponse Response => this.httpContext.Response;

        public IStorageCreator Storage => this.httpContext.Storage;

        public IDictionary<string, string> Session { get => this.httpContext.Session; set => this.httpContext.Session = value; }

        public IDictionary<string, string> LogData => this.httpContext.LogData;

        public ILoadedModuleInfo LoadedModules => this.httpContext.LoadedModules;

        public Task LogMessageAsync(LogLevel level, string message, System.Exception ex)
        {
            return this.httpContext.LogMessageAsync(level, message, ex);
        }
    }
}