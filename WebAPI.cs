
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BepInEx;
using Ceen;
using UnityEngine;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        public static WebAPIPlugin Instance;

        private readonly WebServer _webServer = new WebServer();
        private readonly WebRouter _router = new WebRouter();

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

        void Awake()
        {
            WebAPIPlugin.Instance = this;
            Log("Hello World");

            WebAPI.Config.LoadConfig();
            Dispatcher.Initialize();

            if (WebAPI.Config.Enabled)
            {
                var webRouteType = typeof(IWebRoute);
                var foundTypes = typeof(IWebRoute).Assembly.GetTypes()
                    .Where(p => p.IsClass && p.GetInterfaces().Contains(webRouteType))
                    .ToArray();

                foreach (var routeType in foundTypes)
                {
                    var instance = (IWebRoute)Activator.CreateInstance(routeType);
                    _router.AddRoute(instance);
                }

                _webServer.Start(OnRequest);

                Logging.Log(
                    new Dictionary<string, string>() {
                        {"RouteCount", foundTypes.Length.ToString()}
                    },
                    "API Server started"
                );
            }
            else
            {
                Logging.Log("API Server not started as it has been disabled.");
            }
        }

        async Task<bool> OnRequest(IHttpContext context)
        {
            // For a proper implementation of CORS, see https://github.com/expressjs/cors/blob/master/lib/index.js#L159

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                // TODO: Choose based on available routes at this path
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, DELETE");
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
                context.Response.AddHeader("Access-Control-Max-Age", "1728000");
                context.Response.AddHeader("Access-Control-Expose-Headers", "Authorization");
                context.Response.StatusCode = HttpStatusCode.NoContent;
                context.Response.Headers["Content-Length"] = "0";
                return true;
            }
            else
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Expose-Headers", "Authorization");
            }

            try
            {
                return await _router.HandleRequest(context);
            }
            catch (AuthenticationException e)
            {
                await context.SendResponse(e.StatusCode, new ErrorPayload()
                {
                    message = e.Message
                });
                return true;
            }
            catch (Exception e)
            {
                Logging.Log(e.ToString());
                await context.SendResponse(HttpStatusCode.InternalServerError, new ErrorPayload()
                {
                    message = e.ToString()
                });
                return true;
            }
        }
    }
}