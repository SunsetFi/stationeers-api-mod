using JWT.Builder;
using Newtonsoft.Json;
using StationeersWebApi.Server;

namespace StationeersWebApi.Authentication.Strategies.Steam
{
    public class SteamApiUser : ApiUser
    {
        [JsonProperty("steamId")]
        public string SteamID { get; set; }

        public SteamApiUser()
         : base(Authentication.AuthenticationMethod.Steam)
        {
        }

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