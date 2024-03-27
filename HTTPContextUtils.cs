using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StationeersWebApi.Server;

namespace StationeersWebApi
{
    public static class HTTPContextUtils
    {
        public static T ParseBody<T>(this IHttpContext context)
        {
            var body = context.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static object ParseBody(this IHttpContext context, Type type)
        {
            var body = context.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();

            return JsonConvert.DeserializeObject(text, type);
        }

        public static JToken ParseJson(this IHttpContext context)
        {
            var body = context.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JToken.Parse(text);
        }
    }
}