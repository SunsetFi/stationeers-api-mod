
using System;
using System.Collections.Generic;
using JWT.Builder;
using JWT.Algorithms;
using Ceen;
using System.Threading.Tasks;
using Ceen.Httpd;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using JWT;
using WebAPI.Server.Exceptions;
using WebAPI.Authentication.Strategies.Steam;
using Newtonsoft.Json.Linq;
using WebAPI.Authentication.Strategies.Anonymous;
using WebAPI.Authentication.Strategies.Password;

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

        private static Dictionary<string, IAuthenticationStrategy> _authenticationStrategies = new Dictionary<string, IAuthenticationStrategy> {
            {AuthenticationMethod.Anonymous, new AnonymousAuthenticationStrategy()},
            {AuthenticationMethod.Password, new PasswordAuthenticationStrategy()},
            {AuthenticationMethod.Steam, new SteamAuthenticationStrategy()}
        };


        public static async Task<ApiUser> Authenticate(IHttpContext context, string strategyType)
        {
            IAuthenticationStrategy strategy;
            if (!_authenticationStrategies.TryGetValue(strategyType, out strategy))
            {
                throw new ArgumentException($"Unrecognize auth strategy {strategyType}");
            }
            var user = await strategy.TryAuthenticate(context);
            return user;
        }

        public static ApiUser VerifyAuth(IHttpContext context)
        {
            var token = GetJWTToken(context);

            string authenticationMethod;
            if (token == null)
            {
                authenticationMethod = AuthenticationMethod.Anonymous;
            }
            else
            {
                var unknownUser = token.ToObject<ApiUser>();
                authenticationMethod = unknownUser.AuthenticationMethod;
            }

            if (_authenticationStrategies.TryGetValue(authenticationMethod, out var strategy))
            {
                ApiUser user;
                strategy.Verify(context, token, out user);
            }

            throw new ForbiddenException();
        }

        public static string GenerateToken(ApiUser user)
        {
            var builder = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Config.Instance.JWTSecret)
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
                Secure = Config.Instance.Protocol == "https"
            });
        }

        public static JObject GetJWTToken(IHttpContext context)
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
                return null;
            }

            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(Config.Instance.JWTSecret)
                    .MustVerifySignature()
                    .Decode(token);

                var payload = JObject.Parse(json);
                return payload;
            }
            catch (JsonException)
            {
                throw new UnauthorizedException("Malformed Token.");
            }
            catch (TokenExpiredException)
            {
                throw new UnauthorizedException("Token Expired.");
            }
            catch (SignatureVerificationException)
            {
                throw new UnauthorizedException("Invalid Signature.");
            }
        }
    }
}