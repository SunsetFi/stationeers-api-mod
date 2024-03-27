namespace StationeersWebApi.Server.Attributes
{
    using System;

    /// <summary>
    /// Attribute to mark a method as being a web route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class WebRouteMethodAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the path for this route.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the method for this route.
        /// </summary>
        public string Method { get; set; } = "GET";
    }
}
