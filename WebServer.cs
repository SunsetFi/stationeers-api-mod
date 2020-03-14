
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Ceen;
using Ceen.Httpd;

namespace WebAPI
{
    public class RequestEventArgs : EventArgs
    {
        public HttpListenerContext Context { get; private set; }

        public string Body { get; private set; }

        public RequestEventArgs(HttpListenerContext context, string body)
        {
            this.Context = context;
            this.Body = body;
        }
    }
    public class WebServer
    {
        static void Log(string message)
        {
            WebAPIPlugin.Instance.Log("[WebServer]: " + message);
        }

        public event EventHandler<RequestEventArgs> OnRequest;

        public void Start()
        {
            WebServer.Log("Starting web server");


            var tcs = new CancellationTokenSource();
            var config = new ServerConfig()
                .AddLogger(OnLogMessage)
                .AddRoute(
                    Ceen.MvcExtensionMethods.ToRoute(
                        typeof(WebAPIPlugin).Assembly,
                        new Ceen.Mvc.ControllerRouterConfig()
                    )
                );

            var task = HttpServer.ListenAsync(
                new IPEndPoint(IPAddress.Any, 8080),
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