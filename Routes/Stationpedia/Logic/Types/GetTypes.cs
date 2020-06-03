using System.Collections.Generic;
using WebAPI.Payloads;

namespace WebAPI.Routes.Stationpedia.Logic.Types
{
    class GetTypes : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "logic", "types" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {            
            e.Context.SendResponse(200, LogicTypesPayload.FromGame());
        }
    }
}