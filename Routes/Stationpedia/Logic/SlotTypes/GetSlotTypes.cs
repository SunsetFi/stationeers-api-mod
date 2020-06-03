using System.Collections.Generic;
using WebAPI.Payloads;

namespace WebAPI.Routes.Stationpedia.Logic.SlotTypes
{
    class GetSlotTypes : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "logic", "slottypes" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {            
            e.Context.SendResponse(200, LogicSlotTypesPayload.FromGame());
        }
    }
}