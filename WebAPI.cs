
using System;
using System.Linq;
using System.Net;
using BepInEx;
using UnityEngine;
using WebAPI.Payloads;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        public static WebAPIPlugin Instance;

        private readonly WebServer _webServer = new WebServer();
        private readonly WebRouter _router = new WebRouter();

        public void Log(string line)
        {
            Debug.Log("[WebAPI]: " + line);
        }

        void Awake()
        {
            WebAPIPlugin.Instance = this;
            Log("Hello World");

            WebAPI.Config.LoadConfig();
            Dispatcher.Initialize();

            if (WebAPI.Config.Instance.enabled)
            {


                var webRouteType = typeof(IWebRoute);
                var foundTypes = typeof(IWebRoute).Assembly.GetTypes()
                    .Where(p => p.IsClass && p.GetInterfaces().Contains(webRouteType))
                    .ToArray();

                Log("Initializing " + foundTypes.Length + " routes.");
                foreach (var routeType in foundTypes)
                {
                    var instance = (IWebRoute)Activator.CreateInstance(routeType);
                    _router.AddRoute(instance);
                }
                Log("Routes initialized");

                _webServer.OnRequest += OnWebRequest;
                _webServer.Start();
            }
            else
            {
                Log("Not enabled.");
            }
        }

        void OnWebRequest(object sender, RequestEventArgs e)
        {
            Dispatcher.RunOnMainThread(() => HandleRequest(e));
        }

        void HandleRequest(RequestEventArgs e)
        {
            try
            {
                var handled = _router.HandleRequest(e);

                if (!handled)
                {
                    e.Context.SendResponse(404, new ErrorPayload()
                    {
                        message = "Route not found"
                    });
                }
            }
            catch (Exception ex)
            {
                Log("Failed to handle request: " + ex.ToString());
                e.Context.SendResponse(500, new ErrorPayload()
                {
                    message = ex.ToString()
                });
            }
        }
    }
}