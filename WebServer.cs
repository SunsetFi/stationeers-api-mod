
using System;
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

<<<<<<< HEAD
            if (context.Request.HttpMethod == "OPTIONS")
            {
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                context.Response.AddHeader("Access-Control-Max-Age", "1728000");
                context.Response.StatusCode = 200;
                context.Response.Close();
                return;
            }

            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var password = Config.Instance.password;
            if (password != null && password.Length > 0)
            {
                var suppliedPassword = context.Request.QueryString.Get("password");
                if (suppliedPassword != password)
                {
                    context.SendResponse(401, new ErrorPayload()
                    {
                        message = "Access Denied."
                    });
                    return;
                }
            }
=======
>>>>>>> develop

            var tcs = new CancellationTokenSource();
            var config = new ServerConfig()
                .AddLogger(OnLogMessage)
                .AddRoute(handler);

            var task = HttpServer.ListenAsync(
                new IPEndPoint(IPAddress.Any, Config.Instance.port),
                false,
                config,
                tcs.Token
            );

            WebServer.Log("Server started");
        }

        Task OnLogMessage(IHttpContext context, Exception exception, DateTime started, TimeSpan duration)
        {
            WebServer.Log(exception.ToString());
            return Task.CompletedTask;
        }
    }
}