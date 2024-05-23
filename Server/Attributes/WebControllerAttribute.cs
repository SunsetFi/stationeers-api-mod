namespace StationeersWebApi.Server.Attributes
{
    /// <summary>
    /// Attribute to mark a class as being a web controller.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class WebControllerAttribute : System.Attribute
    {
        /// <summary>
        /// Gets or sets the path for the controller.
        /// </summary>
        public string Path { get; set; }
    }
}
