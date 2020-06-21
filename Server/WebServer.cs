

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Ceen;
using Ceen.Httpd;
using WebAPI.Payloads;

namespace WebAPI.Server
{
    public class WebServer
    {
        private WebRouter _router;

        static void Log(string message)
        {
            WebAPIPlugin.Instance.Log("[WebServer]: " + message);
        }

        public int RouteCount
        {
            get
            {
                return this._router.Count;
            }
        }

        public void AddRoute(IWebRoute route)
        {
            this._router.AddRoute(route);
        }

        public void Start()
        {
            WebServer.Log("Starting web server");

            var tcs = new CancellationTokenSource();
            var config = new ServerConfig()
                .AddLogger(OnLogMessage)
                .AddRoute(this.OnRequest);

            var task = HttpServer.ListenAsync(
                new IPEndPoint(IPAddress.Any, Config.Port),
                false,
                config,
                tcs.Token
            );

            WebServer.Log("Server started");
        }

        private async Task<bool> OnRequest(IHttpContext context)
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
                context.Response.StatusCode = Ceen.HttpStatusCode.NoContent;
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
            catch (Exceptions.WebException e)
            {
                await context.SendResponse(e.StatusCode, new ErrorPayload
                {
                    message = e.Message
                });
                return true;
            }
            catch (Exception e)
            {
                Logging.Log(e.ToString());
                await context.SendResponse(Ceen.HttpStatusCode.InternalServerError, new ErrorPayload
                {
                    message = e.Message
                });
                return true;
            }
        }

        private Task OnLogMessage(IHttpContext context, Exception exception, DateTime started, TimeSpan duration)
        {
            if (exception != null)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                    {"RequestMethod", context.Request.Method},
                    {"RequestPath", context.Request.Path},
                    {"RemoteEndpoint", context.Request.RemoteEndPoint.ToString()}
                    },
                    exception.ToString()
                );
            }
            return Task.CompletedTask;
        }
    }
}