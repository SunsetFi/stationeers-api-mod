
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Router.Attributes;

namespace WebAPI.Server
{
    public class WebControllerRouter : IWebRoute
    {
        private object instance;
        private MethodBase handler;

        public WebControllerRouter(object instance, MethodBase method, string rootPath)
        {
            var attr = method.GetCustomAttribute(typeof(WebRouteMethodAttribute)) as WebRouteMethodAttribute;
            if (attr == null)
            {
                throw new ArgumentException("Expected method to have WebRouteMethodAttribute.");
            }

            this.handler = method;
            this.Method = attr.Method;
            this.Path = System.IO.Path.Combine(rootPath, attr.Path).Replace("\\", "/");
        }

        public string Method { get; private set; }

        public string Path { get; private set; }

        public Task OnRequested(IWebRouteContext context)
        {
            var paramInfos = this.handler.GetParameters();
            var paramValues = paramInfos.Select(paramInfo => this.GetParameterValue(paramInfo, context)).ToArray();
            return (Task)this.handler.Invoke(instance, paramValues);
        }

        private object GetParameterValue(ParameterInfo parameterInfo, IWebRouteContext context)
        {
            switch (parameterInfo.Name)
            {
                case "context":
                    return context;
                case "request":
                    return context.Request;
                case "response":
                    return context.Response;
            }

            if (context.PathParameters.ContainsKey(parameterInfo.Name))
            {
                return WebControllerRouter.ConvertParameter(parameterInfo.ParameterType, context.PathParameters[parameterInfo.Name]);
            }

            throw new Exception(string.Format("Unable to determine value for parameter '{0} of route method '{1}'", parameterInfo.Name, this.handler.Name));
        }

        private static object ConvertParameter(Type targetType, string value)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (targetType == typeof(Int16))
            {
                return Convert.ToInt16(value);
            }
            if (targetType == typeof(Int32))
            {
                return Convert.ToInt32(value);
            }
            if (targetType == typeof(Int64))
            {
                return Convert.ToInt64(value);
            }
            if (targetType == typeof(UInt16))
            {
                return Convert.ToUInt16(value);
            }
            if (targetType == typeof(UInt32))
            {
                return Convert.ToUInt32(value);
            }
            if (targetType == typeof(UInt64))
            {
                return Convert.ToUInt64(value);
            }

            if (targetType == typeof(float))
            {
                return (float)Convert.ToDouble(value);
            }
            if (targetType == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            throw new Exception("Unable to convert value to " + targetType.Name);
        }
    }
}