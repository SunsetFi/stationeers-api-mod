using Ceen;
using JWT.Builder;
using Newtonsoft.Json;

namespace WebAPI.Authentication.Strategies.Steam
{
    public class SteamApiUser : ApiUser
    {
        [JsonProperty("steamId")]
        public string SteamID { get; set; }

        public override void SerializeToJwt(JwtBuilder builder)
        {
            base.SerializeToJwt(builder);
            builder.AddClaim("steamId", this.SteamID);
        }

        public static SteamApiUser MakeSteamUser(IHttpContext context, ulong steamId)
        {
            var user = new SteamApiUser
            {
                SteamID = steamId.ToString()
            };
            ApiUser.InitializeUser(user, context);
            return user;
        }
    }
}