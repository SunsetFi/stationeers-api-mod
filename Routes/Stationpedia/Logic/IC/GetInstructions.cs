using System.Collections.Generic;
using WebAPI.Payloads;

namespace WebAPI.Routes.Stationpedia.Logic.IC
{
    class GetInstructions : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "logic", "ic", "instructions" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {            
            e.Context.SendResponse(200, ICInstructionPayload.FromGame());
        }
    }
}