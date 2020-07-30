
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json.Linq;

namespace WebAPI.Authentication.Strategies.Anonymous
{
    public class AnonymousAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            await context.SendResponse(HttpStatusCode.OK);
            return new AnonymousApiUser();
        }

        public void Verify(IHttpContext context, JObject authToken, out ApiUser user)
        {
            user = new AnonymousApiUser();
        }
    }
}