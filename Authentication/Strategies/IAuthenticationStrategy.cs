

using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Authentication
{
    public interface IAuthenticationStrategy
    {
        Task<AuthenticationResult> TryAuthenticate(IHttpContext context);
        bool TryVerify(IHttpContext context, out ApiUser user);
    }

    public class AuthenticationResult
    {
        public ApiUser User { get; private set; }
        public bool Handled { get; private set; }

        public AuthenticationResult(bool handled, ApiUser user)
        {
            this.User = user;
            this.Handled = handled;
        }
    }
}