
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Ceen;
using JWT.Builder;
using JWT;
using Newtonsoft.Json;
using JWT.Algorithms;

namespace WebAPI.Authentication
{
    public static class Authenticator
    {
        private static readonly Regex BearerRegex = new Regex(@"Bearer\s+([^$]+)", RegexOptions.Compiled);
        private static readonly Regex SteamIdRegex = new Regex(@"openid\/id\/([0-9]{17,25})", RegexOptions.Compiled);
        private static readonly Regex IsValidRegex = new Regex(@"(is_valid\s*:\s*true)", RegexOptions.Compiled);

        private const string ProviderUri = @"https://steamcommunity.com/openid/login";

        public static string GenerateToken(ApiUser user)
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Config.Instance.jwtSecret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
            if (user.isRootUser)
            {
                token.AddClaim("isRootUser", true);
            }
            if (user.isSteamUser)
            {
                token.AddClaim("isSteamUser", true);
                token.AddClaim("steamId", user.steamId.ToString());
            }
            if (user.endpoint != null)
            {
                token.AddClaim("endpoint", user.endpoint);
            }
            return token.Encode();
        }

        public static ApiUser VerifyAuth(IHttpContext context)
        {
            if (!Config.Instance.HasAuthentication)
            {
                return new ApiUser()
                {
                    isRootUser = true
                };
            }

            var user = Authenticator.VerifyPlaintextPassword(context);
            if (user != null)
            {
                return user;
            }

            user = Authenticator.VerifyJWTAuthentication(context);
            if (user != null)
            {
                return user;
            }

            throw new AuthenticationException("Unauthorized.");
        }

        private static ApiUser VerifyJWTAuthentication(IHttpContext context)
        {
            if (!Config.Instance.steamAuthentication)
            {
                return null;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                return null;
            }

            var authHeader = context.Request.Headers["Authorization"];
            var match = BearerRegex.Match(authHeader);
            if (!match.Success)
            {
                throw new AuthenticationException("Malformed Authorization Header.");
            }
            var token = match.Groups[1].Value;

            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(Config.Instance.jwtSecret)
                    .MustVerifySignature()
                    .Decode(token);
                var apiUser = JsonConvert.DeserializeObject<ApiUser>(json);

                if (apiUser.isSteamUser)
                {
                    var allowedSteamIds = Config.Instance.allowedSteamIds;
                    if (allowedSteamIds.Length > 0 && !allowedSteamIds.Contains(apiUser.steamId))
                    {
                        throw new AuthenticationException("Unauthorized.");
                    }
                }

                if (apiUser.endpoint != null && apiUser.endpoint != context.Request.RemoteEndPoint.ToString())
                {
                    throw new AuthenticationException("IP Changed.");
                }

                return apiUser;
            }
            catch (TokenExpiredException)
            {
                throw new AuthenticationException("Token Expired.");
            }
            catch (SignatureVerificationException)
            {
                throw new AuthenticationException("Invalid Signature.");
            }
        }

        private static ApiUser VerifyPlaintextPassword(IHttpContext context)
        {
            // This is temporary, should be replaced with steam login and jwt.
            var password = WebAPI.Config.Instance.plaintextPassword;
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }

            var suppliedPassword = context.Request.QueryString["password"];
            if (suppliedPassword != password)
            {
                throw new AuthenticationException("Unauthorized.");
            }

            return new ApiUser()
            {
                isRootUser = true
            };
        }

        public static ApiUser VerifyLogin(IHttpContext context)
        {
            var queryString = context.Request.QueryString;
            queryString["openid.mode"] = "check_authentication";

            var responseString = HttpPost(ProviderUri, GenerateQuery(queryString));

            var match = SteamIdRegex.Match(queryString["openid.claimed_id"]);
            ulong steamId = ulong.Parse(match.Groups[1].Value);

            match = IsValidRegex.Match(responseString);
            var isValid = match.Success;
            if (!isValid)
            {
                throw new AuthenticationException("Invalid Credentials.");
            }

            var allowedSteamIds = Config.Instance.allowedSteamIds;
            if (allowedSteamIds.Length > 0 && !allowedSteamIds.Contains(steamId.ToString()))
            {
                throw new AuthenticationException("Unauthorized.");
            }

            var user = new ApiUser() { isSteamUser = true, steamId = steamId.ToString(), endpoint = context.Request.RemoteEndPoint.ToString() };
            return user;
        }

        private static string HttpPost(string uri, string content)
        {
            var data = Encoding.ASCII.GetBytes(content);
            var request = WebRequest.Create(ProviderUri);
            request.Method = "POST";
            request.Headers.Add("Accept-language", "en");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;
            request.Timeout = 6000;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private static string GenerateQuery(IDictionary<string, string> collection, bool useAmp = true)
        {
            var parts = (from key in collection.Keys
                         let value = collection[key]
                         select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value)));
            return string.Join(useAmp ? "&" : "&amp;", parts);
        }
    }
}