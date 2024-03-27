namespace StationeersWebApi.Server
{
    using System.Collections.Generic;

    /// <summary>
    /// Http Context for a StationeersWebApi Route request.
    /// </summary>
    public interface IWebRouteHttpContext : IHttpContext
    {
        /// <summary>
        /// Gets the path parameters for this request.
        /// </summary>
        IDictionary<string, string> PathParameters { get; }
    }
}
