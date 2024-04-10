
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/atmospheres")]
    class AtmospheresController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetAtmospheres(IHttpContext context)
        {
            var skip = 0;
            context.QueryString.TryGetValue("skip", out var skipStr);
            if (!string.IsNullOrEmpty(skipStr) && !int.TryParse(skipStr, out skip))
            {
                throw new BadRequestException("Invalid skip value.");
            }

            var take = 0;
            context.QueryString.TryGetValue("take", out var takeStr);
            if (!string.IsNullOrEmpty(takeStr) && !int.TryParse(takeStr, out take))
            {
                throw new BadRequestException("Invalid take value.");
            }

            Authenticator.VerifyAuth(context);

            var atmos = await Dispatcher.RunOnMainThread(() => AtmospheresModel.GetAtmospheres(skip, take));
            await context.SendResponse(HttpStatusCode.OK, atmos);
        }

        [WebRouteMethod(Method = "GET", Path = "rooms")]
        public async Task GetRoomAtmospheres(IHttpContext context)
        {
            var skip = 0;
            context.QueryString.TryGetValue("skip", out var skipStr);
            if (!string.IsNullOrEmpty(skipStr) && !int.TryParse(skipStr, out skip))
            {
                throw new BadRequestException("Invalid skip value.");
            }

            var take = 0;
            context.QueryString.TryGetValue("take", out var takeStr);
            if (!string.IsNullOrEmpty(takeStr) && !int.TryParse(takeStr, out take))
            {
                throw new BadRequestException("Invalid take value.");
            }

            Authenticator.VerifyAuth(context);

            var atmos = await Dispatcher.RunOnMainThread(() => AtmospheresModel.GetRoomAtmospheres(skip, take));
            await context.SendResponse(HttpStatusCode.OK, atmos);
        }

        [WebRouteMethod(Method = "GET", Path = "networks")]
        public async Task GetNetworkAtmospheres(IHttpContext context)
        {
            var skip = 0;
            context.QueryString.TryGetValue("skip", out var skipStr);
            if (!string.IsNullOrEmpty(skipStr) && !int.TryParse(skipStr, out skip))
            {
                throw new BadRequestException("Invalid skip value.");
            }

            var take = 0;
            context.QueryString.TryGetValue("take", out var takeStr);
            if (!string.IsNullOrEmpty(takeStr) && !int.TryParse(takeStr, out take))
            {
                throw new BadRequestException("Invalid take value.");
            }

            Authenticator.VerifyAuth(context);

            var atmos = await Dispatcher.RunOnMainThread(() => AtmospheresModel.GetNetworkAtmospheres(skip, take));
            await context.SendResponse(HttpStatusCode.OK, atmos);
        }
    }
}