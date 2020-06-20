
namespace WebAPI.Server.Attributes
{

    [System.AttributeUsage(System.AttributeTargets.Method)
]
    public class WebRouteMethodAttribute : System.Attribute
    {
        public string Path { get; set; }
        public string Method { get; set; } = "GET";
    }
}