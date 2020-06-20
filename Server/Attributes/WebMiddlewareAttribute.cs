
using System;
using System.Threading.Tasks;

namespace WebAPI.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class WebMiddlewareAttribute : Attribute
    {
        public Func<IWebAPIContext, Task<bool>> Handler { get; private set; }

        public WebMiddlewareAttribute(Func<IWebAPIContext, Task<bool>> handler)
        {
            this.Handler = handler;
        }
    }
}