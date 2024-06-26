
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server;
using System.Threading.Tasks;
using StationeersWebApi.Server.Exceptions;
using StationeersWebApi.Payloads;
using StationeersWebApi.JsonTranslation;
using System.Collections.Generic;

namespace StationeersWebApi
{
    [BepInPlugin("dev.sunsetfi.stationeers.webapi", "Web API for Stationeers", "2.1.1.0")]
    public class StationeersWebApiPlugin : BaseUnityPlugin
    {
        public static StationeersWebApiPlugin Instance;

        private WebServer _webServer;
        private WebRouter _router = new WebRouter();
        private HashSet<string> _registeredAssemblies = new();

        public static string AssemblyDirectory
        {
            get
            {
                var assemblyLocation = typeof(StationeersWebApiPlugin).Assembly.Location;
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                return assemblyDir;
            }
        }

        /// <summary>
        /// Gets the path to the web host content.
        /// </summary>
        public static string WebhostPath
        {
            get
            {
                return Path.Combine(AssemblyDirectory, "web-content");
            }
        }

        public void AddRoute(IWebRoute route)
        {
            this._router.AddRoute(route);
        }

        public void RegisterControllers(Assembly assembly)
        {
            if (!this._registeredAssemblies.Add(assembly.FullName))
            {
                Logging.LogError($"Assembly {assembly.FullName} has already been registered.  Skipping registration.");
                return;
            }

            var controllerTypes = (from type in assembly.GetTypes()
                                   where type.GetCustomAttribute(typeof(WebControllerAttribute)) != null
                                   select type).ToArray();

            var controllerRoutes = (from type in controllerTypes
                                    let controller = Activator.CreateInstance(type)
                                    let routes = WebControllerFactory.CreateRoutesFromController(controller)
                                    from route in routes
                                    select route).ToArray();

            foreach (var route in controllerRoutes)
            {
                this._router.AddRoute(route);
            }

            Logging.LogInfo("Loaded {0} routes from {1} controllers in {2}", controllerRoutes.Length, controllerTypes.Length, assembly.FullName);
        }

        public void StartServer()
        {
            if (!StationeersWebApi.Config.Instance.Enabled)
            {
                return;
            }

            if (this._webServer != null)
            {
                return;
            }

            // The dispatcher game object is deleted on us after the plugin initializes.
            // To account for this, re-initialize it here.  It is safe to call this multiple times,
            // it will only initialize if it has been previously destroyed.
            Dispatcher.Initialize();

            this._webServer = new WebServer(this.OnRequest);
            this._webServer.Start(StationeersWebApi.Config.Instance.Port);
        }

        public void StopServer()
        {
            if (this._webServer != null)
            {
                Logging.LogInfo("Stopping server");
                this._webServer.Dispose();
                this._webServer = null;
            }
        }

        void Awake()
        {
            StationeersWebApiPlugin.Instance = this;

            StationeersWebApi.Config.LoadConfig();
            Dispatcher.Initialize();

            if (StationeersWebApi.Config.Instance.Enabled)
            {
                this.ApplyPatches();

                var ownAssembly = typeof(StationeersWebApiPlugin).Assembly;

                try
                {
                    this.RegisterControllers(ownAssembly);
                }
                catch (Exception ex)
                {
                    Logging.LogInfo("Failed to register default controllers: {0}", ex.Message);
                }

                try
                {
                    JsonTranslator.LoadJsonTranslatorStrategies(ownAssembly);
                }
                catch (Exception ex)
                {
                    Logging.LogInfo("Failed to load JsonTranslator strategies: {0}", ex.Message);
                }
            }
            else
            {
                Logging.LogInfo("StationeersWebApi is disabled.");
            }
        }

        private void ApplyPatches()
        {
            try
            {
                var harmony = new Harmony("net.robophreddev.stationeers.StationeersWebApi");
                harmony.PatchAll();
                Logging.LogInfo("Harmony patching succeeded");
            }
            catch (Exception e)
            {
                Logging.LogError($"Harmony patching failed: {e.StackTrace}");
            }
        }

        private async Task<bool> OnRequest(IHttpContext context)
        {
            // For a proper implementation of CORS, see https://github.com/expressjs/cors/blob/master/lib/index.js#L159

            if (context.Method == "OPTIONS")
            {
                context.SetResponseHeader("Access-Control-Allow-Origin", "*");
                // TODO: Choose based on available routes at this path
                context.SetResponseHeader("Access-Control-Allow-Methods", "GET, POST, DELETE");
                context.SetResponseHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
                context.SetResponseHeader("Access-Control-Max-Age", "1728000");
                context.SetResponseHeader("Access-Control-Expose-Headers", "Authorization");
                context.SetResponseHeader("Content-Length", "0");
                await context.SendResponse(HttpStatusCode.NoContent);
                return true;
            }
            else
            {
                context.SetResponseHeader("Access-Control-Allow-Origin", "*");
                context.SetResponseHeader("Access-Control-Expose-Headers", "Authorization");
            }

            try
            {
                return await _router.HandleRequest(context);
            }
            catch (WebException e)
            {
                await context.SendResponse(e.StatusCode, new ErrorPayload
                {
                    message = e.Message
                });
                return true;
            }
        }
    }
}