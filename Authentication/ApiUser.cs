
using System;
using System.Collections.Generic;
using JWT.Builder;
using Newtonsoft.Json;
using StationeersWebApi;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Exceptions;

namespace WebAPI.Authentication
{
    public class ApiUser
    {
        [JsonProperty("jwtId")]
        public string jwtId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("authenticationMethod")]
        public string AuthenticationMethod { get; set; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        public ApiUser(string authenticationMethod)
        {
            this.AuthenticationMethod = authenticationMethod;
        }

        public virtual void SerializeToJwt(JwtBuilder builder)
        {
            builder.AddClaim("jwtId", this.jwtId);

            builder.AddClaim("authenticationMethod", this.AuthenticationMethod);

            if (!string.IsNullOrEmpty(this.Endpoint))
            {
                builder.AddClaim("endpoint", this.Endpoint);
            }
        }

        public static void InitializeUser(ApiUser user, IHttpContext context)
        {
            user.Endpoint = context.RemoteEndPoint.ToPortlessString();
        }

        public static void VerifyUser(ApiUser user, IHttpContext context)
        {
            var endpoint = context.RemoteEndPoint.ToPortlessString();
            if (user.Endpoint != null && user.Endpoint != endpoint)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                            { "RequestEndpoint", endpoint },
                            { "JWTEndpoint", user.Endpoint }
                    },
                    "JWT login request does not match the source endpoint."
                );
                throw new UnauthorizedException("IP Changed.");
            }
        }
    }
}