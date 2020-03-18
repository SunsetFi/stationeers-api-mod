

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Ceen;
using Ceen.Httpd;

namespace WebAPI
{
    public class WebServer
    {
        static void Log(string message)
        {
            WebAPIPlugin.Instance.Log("[WebServer]: " + message);
        }

        public void Start(HttpHandlerDelegate handler)
        {
            WebServer.Log("Starting web server");

            var tcs = new CancellationTokenSource();
            var config = new ServerConfig()
                .AddLogger(OnLogMessage)
                .AddRoute(handler);

            var task = HttpServer.ListenAsync(
                new IPEndPoint(IPAddress.Any, Config.Port),
                false,
                config,
                tcs.Token
            );

            WebServer.Log("Server started");
        }

        Task OnLogMessage(IHttpContext context, Exception exception, DateTime started, TimeSpan duration)
        {
            Logging.Log(
                new Dictionary<string, string>() {
                    {"RequestMethod", context.Request.Method},
                    {"RequestPath", context.Request.Path},
                    {"RemoteEndpoint", context.Request.RemoteEndPoint.ToString()}
                },
                exception.ToString()
            );
            return Task.CompletedTask;
        }
    }
}