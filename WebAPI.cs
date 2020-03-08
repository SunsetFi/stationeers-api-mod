
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using BepInEx;
using Newtonsoft.Json;
using UnityEngine;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        private HttpListener _listener;
        private Thread _listenerThread;
        volatile bool _queued = false;
        List<Action> _backlog = new List<Action>(8);
        List<Action> _actions = new List<Action>(8);

        void Log(string line)
        {
            Debug.Log("[WebAPI]: " + line);
        }

        void Awake()
        {
            Log("Hello World");
            StartServer();
        }

        void Update()
        {
            if (_queued)
            {
                lock (_backlog)
                {
                    var tmp = _actions;
                    _actions = _backlog;
                    _backlog = tmp;
                    _queued = false;
                }

                foreach (var action in _actions)
                    action();

                _actions.Clear();
            }
        }

        public void RunOnMainThread(Action action)
        {
            lock (_backlog)
            {
                _backlog.Add(action);
                _queued = true;
            }
        }

        void StartServer()
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
            RunOnMainThread(() => HandleRequest(context));
        }

        void HandleRequest(HttpListenerContext context)
        {
            // Log wont work here, probably threading related.
            Log("Got web request");

            var request = context.Request;
            var response = context.Response;
            try
            {
                // TODO: Get / make a routing library
                if (request.Url.Segments.Length >= 3 && request.Url.Segments[1] == "logic/" && request.Url.Segments[2] == "transmitters")
                {
                    var transmittersJson = DumpTransmitters();
                    Respond(response, 200, transmittersJson);
                    // var transmitterJson = DumpTransmitters();
                    // Respond(response, 200, transmitterJson);
                    return;
                }

                Respond(response, 404, "{\"error\": \"Page not found\"}");
            }
            catch (Exception ex)
            {
                Log("Got error: " + ex.ToString());
                var errorMessage = ex.ToString().Replace("\n", "\\n");
                Respond(response, 500, String.Format("{{\"error\": \"{0}\"}}", errorMessage));
            }
        }

        void Respond(HttpListenerResponse response, int statusCode, string json)
        {
            response.StatusCode = statusCode;
            response.SendChunked = false;

            response.Headers.Add("Content-Type", "application/json");

            System.Text.Encoding encoding = response.ContentEncoding;
            if (encoding == null)
            {
                encoding = System.Text.Encoding.UTF8;
                response.ContentEncoding = encoding;
            }

            byte[] buffer = encoding.GetBytes(json);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            response.Close();
        }


        string DumpTransmitters()
        {
            var netObjects = Transmitters.AllTransmitters.Select(logic => LogicToNetObj(logic)).ToArray();
            return JsonConvert.SerializeObject(netObjects, Formatting.Indented);
        }

        LogicNetObj LogicToNetObj(ILogicable transmitter)
        {
            var logicValues = new Dictionary<string, double>();
            foreach (LogicType logicType in Enum.GetValues(typeof(LogicType)))
            {
                if (transmitter.CanLogicRead(logicType))
                {
                    var value = transmitter.GetLogicValue(logicType);
                    logicValues.Add(logicType.ToString(), value);
                }
            }

            return new LogicNetObj
            {
                name = transmitter.DisplayName,
                logicValues = logicValues
            };
        }
    }

    public class LogicNetObj
    {
        public string name;
        public Dictionary<string, double> logicValues;
    }

}