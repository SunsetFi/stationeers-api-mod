
using System;
using System.Collections.Generic;
using JWT.Builder;
using JWT.Algorithms;
using Ceen;
using WebAPI.Authentication.Strategies;
using System.Threading.Tasks;
using Ceen.Httpd;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using JWT;
using System.Linq;

namespace WebAPI.Authentication
{
    public static class Authenticator
    {
        private static readonly Regex BearerRegex = new Regex(@"Bearer\s+([^$]+)", RegexOptions.Compiled);

        private static DateTime TokenExpireTime
        {
            get
            {
                return DateTime.UtcNow.AddHours(1);
            }
        }

        // TODO: Set from config
        private static IAuthenticationStrategy _authenticationStrategy;
        private static IAuthenticationStrategy GetAuthenticationStrategy()
        {
            if (_authenticationStrategy == null)
            {
                switch (Config.AuthenticationMode)
                {
                    case AuthenticationMode.None:
                        _authenticationStrategy = new NoneAuthenticationStrategy();
                        break;
                    case AuthenticationMode.Steam:
                        _authenticationStrategy = new SteamAuthenticationStrategy();
                        break;
                    default:
                        throw new Exception("No authentication mode configured.");
                }
            }
            return _authenticationStrategy;
        }

        public static async Task<ApiUser> Authenticate(IHttpContext context)
        {
            var authenticationStrategy = GetAuthenticationStrategy();
            var result = await authenticationStrategy.TryAuthenticate(context);
            if (!result.Handled)
            {
                await context.SendResponse(HttpStatusCode.Unauthorized, "Unauthorized.");
                return null;
            }

            return result.User;
        }

        public static ApiUser VerifyAuth(IHttpContext context)
        {
            var authenticationStrategy = GetAuthenticationStrategy();

            ApiUser user;
            if (!authenticationStrategy.TryVerify(context, out user))
            {
                throw new AuthenticationException(HttpStatusCode.Unauthorized, "Unauthorized.");
            }

            return user;
        }


        public static string GenerateToken(ApiUser user)
        {
            var builder = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Config.JWTSecret)
                .AddClaim("exp", new DateTimeOffset(Authenticator.TokenExpireTime).ToUnixTimeSeconds());
            user.SerializeToJwt(builder);
            return builder.Encode();
        }

        public static void SetUserToken(IHttpContext context, ApiUser user)
        {
            var token = Authenticator.GenerateToken(user);
            context.Response.Cookies.Add(new ResponseCookie("Authorization", token)
            {
                Expires = Authenticator.TokenExpireTime,
                HttpOnly = true,
                Secure = Config.Protocol == "https"
            });
        }

        public static ApiUser GetUserFromToken(IHttpContext context)
        {
            string token = null;
            if (context.Request.Cookies.ContainsKey("Authorization"))
            {
                token = context.Request.Cookies["Authorization"];
            }
            else if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"];
                var match = BearerRegex.Match(authHeader);
                if (match.Success)
                {
                    token = match.Groups[1].Value;
                }
            }

            if (token == null)
            {
                throw new AuthenticationException(Ceen.HttpStatusCode.Unauthorized, "Unauthorized.");
            }

            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(Config.JWTSecret)
                    .MustVerifySignature()
                    .Decode(token);
                var apiUser = JsonConvert.DeserializeObject<ApiUser>(json);

                VerifySteamUser(apiUser);
                VerifyEndpoint(context, apiUser);

                return apiUser;
            }
            catch (JsonException)
            {
                throw new AuthenticationException(Ceen.HttpStatusCode.Unauthorized, "Malformed Token.");
            }
            catch (TokenExpiredException)
            {
                throw new AuthenticationException(Ceen.HttpStatusCode.Unauthorized, "Token Expired.");
            }
            catch (SignatureVerificationException)
            {
                throw new AuthenticationException(Ceen.HttpStatusCode.Unauthorized, "Invalid Signature.");
            }
        }


        private static void VerifyEndpoint(IHttpContext context, ApiUser apiUser)
        {
            var endpoint = context.Request.RemoteEndPoint.ToPortlessString();
            if (apiUser.endpoint != null && apiUser.endpoint != endpoint)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                            { "RequestEndpoint", endpoint },
                            { "JWTEndpoint", apiUser.endpoint }
                    },
                    "JWT login request does not match the source endpoint."
                );
                throw new AuthenticationException(Ceen.HttpStatusCode.Unauthorized, "IP Changed.");
            }
        }

        private static void VerifySteamUser(ApiUser apiUser)
        {
            if (apiUser.isSteamUser)
            {
                var allowedSteamIds = Config.AllowedSteamIds;
                if (allowedSteamIds.Length > 0 && !allowedSteamIds.Contains(apiUser.steamId))
                {
                    Logging.Log(
                        new Dictionary<string, string>() {
                                { "SteamID", apiUser.steamId }
                        },
                        "JWT login request contained unauthenticated SteamID."
                    );
                    throw new AuthenticationException(Ceen.HttpStatusCode.Forbidden, "Forbidden.");
                }
            }
        }
    }
}