
using System.Collections.Generic;
using Ceen;

namespace WebAPI.Server
{
    public interface IWebRouteContext : IHttpContext
    {
        IDictionary<string, string> PathParameters { get; set; }
    }
}