
namespace WebAPI.Server.Attributes
{

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class WebControllerAttribute : System.Attribute
    {
        public string Path { get; set; }
    }
}