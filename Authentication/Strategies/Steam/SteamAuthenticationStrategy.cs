
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StationeersWebApi;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Authentication.Strategies.Steam
{
    public class SteamAuthenticationStrategy : IAuthenticationStrategy
    {
        private const string ProviderUri = @"https://steamcommunity.com/openid/login";

        private static readonly Regex SteamIdRegex = new Regex(@"openid\/id\/([0-9]{17,25})", RegexOptions.Compiled);
        private static readonly Regex IsValidRegex = new Regex(@"(is_valid\s*:\s*true)", RegexOptions.Compiled);

        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            if (!Config.Instance.SteamAuthentication.Enabled)
            {
                throw new NotFoundException();
            }

            if (context.Method != "GET")
            {
                // openid requests use GET
                throw new MethodNotAllowedException();
            }

            var queryString = context.QueryString.ToDictionary(x => x.Key, x => x.Value);

            if (queryString.Count == 0)
            {
                // Redirect to steam's openid endpoint
                var queryParams = new Dictionary<string, string>() {
                    {"openid.ns", "http://specs.openid.net/auth/2.0"},
                    {"openid.mode", "checkid_setup"},
                    // Normally, we would have the response come back to us, but we are using jwt and not session data.
                    //  This means the client has to make the call to us, not steam, as the client will need to receive the
                    //  resulting authorization header.
                    // These are left blank, and the client must fill them in on its own.
                    //{"openid.return_to", string.Format("{0}://{1}:{2}/login", Config.Protocol, Server.IP, Config.Port)},
                    //{"openid.realm", string.Format("{0}://{1}:{2}", Config.Protocol, Server.IP, Config.Port)},
                    {"openid.identity", "http://specs.openid.net/auth/2.0/identifier_select"},
                    {"openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select"}
                };
                var query = GenerateQuery(queryParams);

                // Would be nice to use TemporarilyMoved here, but thats auto handled by browsers
                //  and there is no way for the client to receive the location being redirected.
                // context.Response.Headers.Add("Location", string.Format("{0}?{1}", ProviderUri, query));
                await context.SendResponse(StationeersWebApi.Server.HttpStatusCode.OK, new AuthenticateWithSteamPayload()
                {
                    location = string.Format("{0}?{1}", ProviderUri, query)
                });
                return null;
            }

            queryString["openid.mode"] = "check_authentication";

            var responseString = HttpPost(ProviderUri, GenerateQuery(queryString));

            var match = SteamIdRegex.Match(queryString["openid.claimed_id"]);
            ulong steamId = ulong.Parse(match.Groups[1].Value);

            match = IsValidRegex.Match(responseString);
            var isValid = match.Success;
            if (!isValid)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                        { "SteamID", steamId.ToString() }
                    },
                    "Steam rejected the provided openid credentials."
                );
                throw new ForbiddenException("Invalid Credentials.");
            }

            var allowedSteamIds = Config.Instance.SteamAuthentication.AllowedSteamIds;
            if (allowedSteamIds.Length > 0 && !allowedSteamIds.Contains(steamId.ToString()))
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                        { "SteamID", steamId.ToString() }
                    },
                    "Attempted login by a SteamID not in the allow list."
                );
                throw new ForbiddenException();
            }

            var user = SteamApiUser.MakeSteamUser(context, steamId);
            Authenticator.SetUserToken(context, user);
            return user;
        }

        public void Verify(IHttpContext context, JObject authToken, out ApiUser user)
        {
            SteamApiUser steamUser = authToken.ToObject<SteamApiUser>();
            if (steamUser == null)
            {
                throw new ForbiddenException();
            }

            user = steamUser;

            ApiUser.VerifyUser(steamUser, context);

            var allowedSteamIds = Config.Instance.SteamAuthentication.AllowedSteamIds;
            if (allowedSteamIds.Length > 0 && !allowedSteamIds.Contains(steamUser.SteamID))
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                        { "SteamID", steamUser.SteamID }
                    },
                    "JWT login request contained a SteamID not in the allow list."
                );
                throw new ForbiddenException();
            }
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
            var parts = from key in collection.Keys
                        let value = collection[key]
                        select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
            return string.Join(useAmp ? "&" : "&amp;", parts);
        }
    }
}