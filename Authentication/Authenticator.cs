
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace WebAPI.Authentication
{
    public static class Authenticator
    {
        private static readonly Regex SteamIdRegex = new Regex(@"openid\/id\/([0-9]{17,25})", RegexOptions.Compiled);
        private static readonly Regex IsValidRegex = new Regex(@"(is_valid\s*:\s*true)", RegexOptions.Compiled);

        private const string ProviderUri = @"https://steamcommunity.com/openid/login";

        public static ApiUser VerifyLogin(Uri requestUri)
        {
            var openIdSpecs = ParseQuery(requestUri.Query);
            openIdSpecs["openid.mode"] = "check_authentication";

            var responseString = HttpPost(ProviderUri, GenerateQuery(openIdSpecs));

            var match = SteamIdRegex.Match(openIdSpecs["openid.claimed_id"]);
            ulong steamId = ulong.Parse(match.Groups[1].Value);

            match = IsValidRegex.Match(responseString);
            var isValid = match.Success;
            if (!isValid)
            {
                throw new AuthenticationException();
            }

            var user = new ApiUser() { steamId = steamId };
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

        private static string GenerateQuery(Dictionary<string, string> collection, bool useAmp = true)
        {
            var parts = (from key in collection.Keys
                         let value = collection[key]
                         select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value)));
            return string.Join(useAmp ? "&" : "&amp;", parts);
        }

        private static Dictionary<string, string> ParseQuery(string query)
        {
            var queryParameters = new Dictionary<string, string>();
            string[] querySegments = query.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = Uri.UnescapeDataString(parts[0].Trim(new char[] { '?', ' ' }));
                    string val = Uri.UnescapeDataString(parts[1].Trim());

                    queryParameters.Add(key, val);
                }
            }

            return queryParameters;
        }
    }
}