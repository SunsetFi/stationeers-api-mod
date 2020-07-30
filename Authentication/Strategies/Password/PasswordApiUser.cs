
using System;
using Ceen;
using JWT.Builder;
using Newtonsoft.Json;

namespace WebAPI.Authentication.Strategies.Password
{
    public class PasswordApiUser : ApiUser
    {
        // This exists only to ensure the password has not changed since last authentication.
        // We just use .GetHashCode, which has a high rate of collisions, but we never use this to verify access.
        [JsonProperty("passwordHash")]
        public int PasswordHash { get; set; }

        public override void SerializeToJwt(JwtBuilder builder)
        {
            base.SerializeToJwt(builder);
        }

        public static PasswordApiUser MakePasswordUser(IHttpContext context, string password)
        {
            var user = new PasswordApiUser
            {
                PasswordHash = password.GetHashCode()
            };
            ApiUser.InitializeUser(user, context);
            return user;
        }
    }
}