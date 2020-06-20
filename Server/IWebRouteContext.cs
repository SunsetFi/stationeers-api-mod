
using System.Collections.Generic;
using Ceen;

namespace WebAPI.Server
{
    public interface IWebAPIContext : IHttpContext
    {
        IDictionary<string, string> PathParameters { get; set; }
    }
}