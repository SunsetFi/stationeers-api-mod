
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Server
{
    public class WebControllerRouter : IWebRoute
    {
        private object instance;
        private MethodBase handler;
        private Func<IWebAPIContext, Task<bool>>[] middleware;

        public WebControllerRouter(object instance, MethodBase method, WebRouteMethodAttribute attr, string rootPath)
        {
            var middleware = from middlewareAttr in method.GetCustomAttributes(typeof(WebMiddlewareAttribute)) as WebMiddlewareAttribute[]
                             select middlewareAttr.Handler;
            this.middleware = middleware.ToArray();
            this.handler = method;
            this.Method = attr.Method;
            this.Path = System.IO.Path.Combine(rootPath, attr.Path).Replace("\\", "/");
        }

        public string Method { get; private set; }

        public string Path { get; private set; }

        public async Task OnRequested(IWebAPIContext context)
        {

            foreach (var middleware in this.middleware)
            {
                if (await middleware(context) == false)
                {
                    return;
                }
            }

            var paramInfos = this.handler.GetParameters();
            var paramValues = paramInfos.Select(paramInfo => this.GetParameterValue(paramInfo, context)).ToArray();

            await (Task)this.handler.Invoke(instance, paramValues);
        }

        private object GetParameterValue(ParameterInfo parameterInfo, IWebAPIContext context)
        {
            switch (parameterInfo.Name)
            {
                case "context":
                    return context;
                case "request":
                    return context.Request;
                case "response":
                    return context.Response;
                case "body":
                    {
                        try
                        {
                            return context.ParseBody(parameterInfo.ParameterType);
                        }
                        catch (Exception)
                        {
                            throw new BadRequestException(string.Format("Expected body to be in the format of {0}.", parameterInfo.ParameterType.Name));
                        }
                    }
            }

            if (context.PathParameters.ContainsKey(parameterInfo.Name))
            {
                return WebControllerRouter.ConvertPathParameter(parameterInfo.ParameterType, context.PathParameters[parameterInfo.Name]);
            }

            throw new Exception(string.Format("Unable to determine value for parameter '{0} of route method '{1}'", parameterInfo.Name, this.handler.Name));
        }

        private static object ConvertPathParameter(Type targetType, string value)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            try
            {
                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, value);
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
            }
            catch (Exception)
            {
                throw new NotFoundException();
            }

            throw new Exception("Unable to convert value to " + targetType.Name);
        }
    }
}