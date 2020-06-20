
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using WebAPI.Router.Attributes;
using WebAPI.Server;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        public static WebAPIPlugin Instance;

        private readonly WebServer _webServer = new WebServer();

        public static string AssemblyDirectory
        {
            get
            {
                var assemblyLocation = typeof(WebAPIPlugin).Assembly.Location;
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                return assemblyDir;
            }
        }

        public void Log(string line)
        {
            Debug.Log("[WebAPI]: " + line);
        }

        public void AddRoute(IWebRoute route)
        {
            this._webServer.AddRoute(route);
        }

        public void RegisterControllers(Assembly assembly)
        {
            var controllerRoutes = from type in assembly.GetTypes()
                                   where type.GetCustomAttribute(typeof(WebControllerAttribute)) != null
                                   let controller = Activator.CreateInstance(type)
                                   let routes = WebControllerFactory.CreateRoutesFromController(controller)
                                   from route in routes
                                   select route;

            foreach (var route in controllerRoutes)
            {
                this._webServer.AddRoute(route);
            }
        }

        void Awake()
        {
            WebAPIPlugin.Instance = this;
            Log("Hello World");

            WebAPI.Config.LoadConfig();
            Dispatcher.Initialize();

            if (WebAPI.Config.Enabled)
            {

                this.ApplyPatches();
                this.RegisterRoutes();
                this.RegisterControllers(typeof(WebAPIPlugin).Assembly);


                _webServer.Start();

                Logging.Log(
                    new Dictionary<string, string>() {
                        {"RouteCount", this._webServer.RouteCount.ToString()}
                    },
                    "API Server started"
                );
            }
            else
            {
                Logging.Log("API Server not started as it has been disabled.");
            }
        }

        private void ApplyPatches()
        {
            var harmony = new Harmony("net.robophreddev.stationeers.WebAPI");
            harmony.PatchAll();
            Log("Patch succeeded");
        }

        private void RegisterRoutes()
        {
            var webRouteType = typeof(IWebRoute);
            var foundTypes = typeof(IWebRoute).Assembly.GetTypes()
                .Where(p => p.IsClass && p.GetInterfaces().Contains(webRouteType))
                .ToArray();

            foreach (var routeType in foundTypes)
            {
                var instance = (IWebRoute)Activator.CreateInstance(routeType);
                this._webServer.AddRoute(instance);
            }
        }
    }
}