namespace StationeersWebApi.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using StationeersWebApi.Server.Attributes;

    /// <summary>
    /// Factory for creating routes from a controller.
    /// </summary>
    public static class WebControllerFactory
    {
        /// <summary>
        /// Creates routes from a controller.
        /// </summary>
        /// <param name="instance">The instance of the controller to create routes from.</param>
        /// <returns>An enumerable of routes handling the controller paths and methods.</returns>
        public static IEnumerable<IWebRoute> CreateRoutesFromController(object instance)
        {
            var type = instance.GetType();
            var controllerAttribute = Attribute.GetCustomAttribute(type, typeof(WebControllerAttribute)) as WebControllerAttribute;
            if (controllerAttribute == null)
            {
                throw new Exception("Object of type " + type.FullName + " does not have a WebControllerAttribute");
            }

            var routeables = from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
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
