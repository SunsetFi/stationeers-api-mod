using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace WebAPI
{
    class WebHookRegistration : IEqualityComparer<WebHookRegistration>
    {
        public string Url { get; set; }

        public WebHookRegistration(string url)
        {
            Url = url;
        }

        public bool Equals(WebHookRegistration x, WebHookRegistration y)
        {
            return x.Url == y.Url;
        }

        public int GetHashCode(WebHookRegistration obj)
        {
            return obj.Url.GetHashCode();
        }
    }
    public static class WebHookEngine
    {
        private static Dictionary<string, HashSet<WebHookRegistration>> registeredHooks = new Dictionary<string, HashSet<WebHookRegistration>>();

        public static void RegisterWebHook(string hookType, string url)
        {
            HashSet<WebHookRegistration> hooks;
            if (!registeredHooks.TryGetValue(hookType, out hooks))
            {
                hooks = new HashSet<WebHookRegistration>();
                registeredHooks.Add(hookType, hooks);
            }

            hooks.Add(new WebHookRegistration(url));
        }

        public static void BroadcastHook(string hookType, object payload)
        {
            HashSet<WebHookRegistration> hooks;
            if (!registeredHooks.TryGetValue(hookType, out hooks))
            {
                return;
            }

            var body = JsonConvert.ToString(payload);
            foreach (var hook in hooks)
            {
                PostWebHook(hook, body);
            }
        }

        private static async void PostWebHook(WebHookRegistration hook, string body)
        {
            var client = new HttpClient();
            var content = new StringContent(body);
            content.Headers.Add("Content-Type", "application/json");
            var response = await client.PostAsync(hook.Url, content);
        }
    }
}