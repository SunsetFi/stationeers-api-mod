
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebAPI.Router.Attributes;

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

            var routeableMethods = from method in type.GetMethods(BindingFlags.Public)
                                   where method.GetCustomAttribute(typeof(WebRouteMethodAttribute)) != null
                                   select method;

            foreach (var method in routeableMethods)
            {
                yield return new WebControllerRouter(instance, method, controllerAttribute.Path);
            }
        }
    }
}