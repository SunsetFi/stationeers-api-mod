
using StationeersWebApi.Server;

namespace WebAPI.Authentication.Strategies.Password
{
    public class PasswordApiUser : ApiUser
    {
        // TODO: Hash and store the password so we can verify the password has not changed since last login.

        public PasswordApiUser()
            : base(Authentication.AuthenticationMethod.Password)
        {
        }

        public static PasswordApiUser MakePasswordUser(IHttpContext context, string password)
        {
            var user = new PasswordApiUser();
            ApiUser.InitializeUser(user, context);
            return user;
        }
    }
}