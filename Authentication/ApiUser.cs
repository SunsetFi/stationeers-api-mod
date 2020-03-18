
using System;
using JWT.Builder;

namespace WebAPI.Authentication
{
    public class ApiUser
    {
        public string jwtId { get; set; } = Guid.NewGuid().ToString();
        public bool isSteamUser { get; set; }
        public string steamId { get; set; }

        public bool isRootUser { get; set; }

        public string endpoint { get; set; }

        public void SerializeToJwt(JwtBuilder builder)
        {
            builder.AddClaim("jwtId", this.jwtId);

            if (this.isRootUser)
            {
                builder.AddClaim("isRootUser", true);
            }

            if (this.isSteamUser)
            {
                builder.AddClaim("isSteamUser", true);
                builder.AddClaim("steamId", this.steamId.ToString());
            }

            if (this.endpoint != null)
            {
                builder.AddClaim("endpoint", this.endpoint);
            }
        }
    }
}