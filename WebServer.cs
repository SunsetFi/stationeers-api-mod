
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Newtonsoft.Json;

namespace WebAPI
{
    public class RequestEventArgs : EventArgs
    {
        public HttpListenerContext Context { get; private set; }

        public RequestEventArgs(HttpListenerContext context)
        {
            this.Context = context;
        }
    }
    public class WebServer
    {
        private HttpListener _listener;
        private Thread _listenerThread;

        public event EventHandler<RequestEventArgs> OnRequest;

        public void Start()
        {
            Log("Starting web server");
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:4444/");
            _listener.Prefixes.Add("http://127.0.0.1:4444/");
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            _listener.Start();

            _listenerThread = new Thread(ListenServer);
            _listenerThread.Start();
            Log("Server started");
        }

        void ListenServer()
        {
            while (true)
            {
                var result = _listener.BeginGetContext(OnWebRequest, _listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        void OnWebRequest(IAsyncResult result)
        {
            var context = _listener.EndGetContext(result);
            if (OnRequest != null)
            {
                OnRequest(this, new RequestEventArgs(context));
            }
        }

        void Log(string message)
        {
            WebAPIPlugin.Instance.Log("[WebServer]: " + message);
        }
    }


}