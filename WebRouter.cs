
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;

namespace WebAPI
{
    public interface IWebRoute
    {
        string Method { get; }
        string[] Segments { get; }
        Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams);


    }

    public class WebRouter
    {
        private List<IWebRoute> _routes = new List<IWebRoute>();

        public void AddRoute(IWebRoute route)
        {
            _routes.Add(route);
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

                var pathParams = MatchRoute(request.Path, route.Segments);
                if (pathParams == null)
                {
                    continue;
                }

                await route.OnRequested(context, pathParams);
                return true;
            }

            return false;
        }

        IDictionary<string, string> MatchRoute(string path, string[] segments)
        {
            var pathSegments = path.Split('/');
            if (pathSegments.Length - 1 != segments.Length)
            {
                return null;
            }

            var pathParams = new Dictionary<string, string>();

            for (var i = 0; i < segments.Length; i++)
            {
                var pathSegment = pathSegments[i];
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
}