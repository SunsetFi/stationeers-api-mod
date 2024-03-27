

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StationeersWebApi.Server;

namespace StationeersWebApi.Authentication
{
    public interface IAuthenticationStrategy
    {
        Task<ApiUser> TryAuthenticate(IHttpContext context);
        void Verify(IHttpContext context, JObject authToken, out ApiUser user);
    }
}