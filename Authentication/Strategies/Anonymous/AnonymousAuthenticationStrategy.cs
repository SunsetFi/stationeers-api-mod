
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StationeersWebApi;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Exceptions;

namespace WebAPI.Authentication.Strategies.Anonymous
{
    public class AnonymousAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            // Only enable anonymous if the user has not configured anything.
            // FIXME: Insecure by default
            if (Config.Instance.PasswordAuthentication.Enabled || Config.Instance.SteamAuthentication.Enabled)
            {
                throw new NotFoundException();
            }

            await context.SendResponse(HttpStatusCode.OK);
            return new AnonymousApiUser();
        }

        public void Verify(IHttpContext context, JObject authToken, out ApiUser user)
        {
            if (Config.Instance.PasswordAuthentication.Enabled || Config.Instance.SteamAuthentication.Enabled)
            {
                throw new UnauthorizedException();
            }

            user = new AnonymousApiUser();
        }
    }
}