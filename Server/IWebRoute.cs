namespace StationeersWebApi.Server
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface describing a web route.
    /// </summary>
    public interface IWebRoute
    {
        /// <summary>
        /// Gets the method for this route.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the path for this route.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the priority of this route.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Handle a request for this route.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>A task to be completed when the request is handled.</returns>
        Task OnRequested(IWebRouteHttpContext context);
    }
}
