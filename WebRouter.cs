
using System;
using System.Collections.Generic;
using System.Net;

namespace WebAPI
{
    public interface IWebRoute
    {
        string Method { get; }
        string[] Segments { get; }
        void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams);


    }

    public class WebRouter
    {
        private List<IWebRoute> _routes = new List<IWebRoute>();

        public void AddRoute(IWebRoute route)
        {
            _routes.Add(route);
        }

        public bool HandleRequest(RequestEventArgs e)
        {
            var request = e.Context.Request;

            var matched = false;
            foreach (var route in _routes)
            {
                if (request.HttpMethod != route.Method)
                {
                    continue;
                }

                var pathParams = MatchRoute(request.Url, route.Segments);
                if (pathParams == null)
                {
                    continue;
                }
                matched = true;
                route.OnRequested(e, pathParams);
            }

            return matched;
        }

        IDictionary<string, string> MatchRoute(Uri uri, string[] segments)
        {
            if (uri.Segments.Length - 1 != segments.Length)
            {
                return null;
            }

            var pathParams = new Dictionary<string, string>();

            for (var i = 0; i < segments.Length; i++)
            {
                var pathSegment = uri.Segments[i + 1];
                if (pathSegment.EndsWith("/"))
                {
                    pathSegment = pathSegment.Substring(0, pathSegment.Length - 1);
                }

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