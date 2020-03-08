
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
using WebAPI.API;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        public static WebAPIPlugin Instance;

        private WebServer _webServer;

        public void Log(string line)
        {
            Debug.Log("[WebAPI]: " + line);
        }

        void Awake()
        {
            WebAPIPlugin.Instance = this;
            Dispatcher.Initialize();
            Log("Hello World");
            _webServer = new WebServer();
            _webServer.OnRequest += OnWebRequest;
            _webServer.Start();
        }

        void OnWebRequest(object sender, RequestEventArgs e)
        {
            Dispatcher.RunOnMainThread(() => HandleRequest(e.Context));
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
                    var transmittersJson = GetAllTransmitters();
                    context.SendResponse(200, transmittersJson);
                    return;
                }

                context.SendResponse(404, new Error()
                {
                    message = "Route not found"
                });
            }
            catch (Exception ex)
            {
                Log("Got error: " + ex.ToString());
                context.SendResponse(500, new Error()
                {
                    message = ex.ToString()
                });
            }
        }

        LogicItem[] GetAllTransmitters()
        {
            return Transmitters.AllTransmitters.Select(logic => LogicToNetObj(logic)).ToArray();
        }

        LogicItem LogicToNetObj(ILogicable transmitter)
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

            return new LogicItem
            {
                name = transmitter.DisplayName,
                logicValues = logicValues
            };
        }
    }
}