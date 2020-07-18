
using System;

namespace WebAPI.Server.Attributes
{

    [AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class WebRouteMethodAttribute : Attribute
    {
        public string Path { get; set; }
        public string Method { get; set; } = "GET";
    }
}