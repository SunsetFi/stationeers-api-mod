namespace StationeersWebApi.Server
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using StationeersWebApi.Server.Attributes;
    using StationeersWebApi.Server.Exceptions;

    /// <summary>
    /// A router that provides routing for a <see cref="WebControllerAttribute"/> marked class.
    /// </summary>
    public class WebControllerRouter : IWebRoute
    {
        private object instance;
        private MethodBase handler;
        private Func<IHttpContext, Task<bool>>[] middleware;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebControllerRouter"/> class.
        /// </summary>
        /// <param name="instance">The web controller instance.</param>
        /// <param name="method">The method to route.</param>
        /// <param name="attr">The attribute describing the method.</param>
        /// <param name="rootPath">The root web path for this controller.</param>
        public WebControllerRouter(object instance, MethodBase method, WebRouteMethodAttribute attr, string rootPath)
        {
            this.instance = instance;

            var middleware = from middlewareAttr in method.GetCustomAttributes(typeof(WebMiddlewareAttribute)) as WebMiddlewareAttribute[]
                             select middlewareAttr.Handler;
            this.middleware = middleware.ToArray();
            this.handler = method;
            this.Method = attr.Method;

            this.Path = rootPath;
            if (attr.Path != null)
            {
                this.Path = System.IO.Path.Combine(this.Path, attr.Path).Replace("\\", "/");
            }
        }

        /// <inheritdoc/>
        public string Method { get; private set; }

        /// <inheritdoc/>
        public string Path { get; private set; }

        /// <inheritdoc/>
        public async Task OnRequested(IWebRouteHttpContext context)
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

            await (Task)this.handler.Invoke(this.instance, paramValues);
        }

        private object GetParameterValue(ParameterInfo parameterInfo, IWebRouteHttpContext context)
        {
            switch (parameterInfo.Name)
            {
                case "context":
                    return context;
                case "body":
                    {
                        try
                        {
                            if (typeof(JToken).IsAssignableFrom(parameterInfo.ParameterType))
                            {
                                return context.ParseJson();
                            }

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
                return this.ConvertPathParameter(parameterInfo.ParameterType, context.PathParameters[parameterInfo.Name]);
            }

            throw new Exception(string.Format("Unable to determine value for parameter '{0} of route method '{1}'", parameterInfo.Name, this.handler.Name));
        }

        private object ConvertPathParameter(Type targetType, string value)
        {
            value = Uri.UnescapeDataString(value);

            if (targetType == typeof(string))
            {
                return value;
            }

            try
            {
                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, value, true);
                }

                if (targetType == typeof(bool))
                {
                    return Convert.ToBoolean(value);
                }

                if (targetType == typeof(short))
                {
                    return Convert.ToInt16(value);
                }

                if (targetType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }

                if (targetType == typeof(long))
                {
                    return Convert.ToInt64(value);
                }

                if (targetType == typeof(ushort))
                {
                    return Convert.ToUInt16(value);
                }

                if (targetType == typeof(uint))
                {
                    return Convert.ToUInt32(value);
                }

                if (targetType == typeof(ulong))
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
