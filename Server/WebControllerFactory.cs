
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebAPI.Server.Attributes;

namespace WebAPI.Server
{
    public static class WebControllerFactory
    {
        public static IEnumerable<IWebRoute> CreateRoutesFromController(object instance)
        {
            var type = instance.GetType();
            var controllerAttribute = Attribute.GetCustomAttribute(type, typeof(WebControllerAttribute)) as WebControllerAttribute;
            if (controllerAttribute == null)
            {
                throw new Exception("Object of type " + type.FullName + " does not have a WebControllerAttribute");
            }

            var routeables = from method in type.GetMethods(BindingFlags.Public)
                             let attrs = method.GetCustomAttributes(typeof(WebRouteMethodAttribute)) as WebRouteMethodAttribute[]
                             from attr in attrs
                             select new { Method = method, Attribute = attr };

            foreach (var routeable in routeables)
            {
                yield return new WebControllerRouter(instance, routeable.Method, routeable.Attribute, controllerAttribute.Path);
            }
        }
    }
}