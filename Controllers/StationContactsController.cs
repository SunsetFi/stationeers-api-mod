
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/stationcontacts")]
    public class StationContactsController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetAllStationContacts(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            var payload = await Dispatcher.RunOnMainThread(() => StationContactsModel.GetStationContacts());
            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "GET", Path = ":contactId")]
        public async Task GetStationContact(IHttpContext context, int contactId)
        {
            Authenticator.VerifyAuth(context);
            var payload = await Dispatcher.RunOnMainThread(() => StationContactsModel.GetStationContact(contactId));
            if (payload == null)
            {
                throw new NotFoundException("StationContact not found.");
            }
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
   }
}