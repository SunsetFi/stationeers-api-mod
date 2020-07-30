

using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json.Linq;

namespace WebAPI.Authentication
{
    public interface IAuthenticationStrategy
    {
        Task<ApiUser> TryAuthenticate(IHttpContext context);
        void Verify(IHttpContext context, JObject authToken, out ApiUser user);
    }
}