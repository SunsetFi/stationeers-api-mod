using System;
using System.IO;
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI
{
    public static class HTTPContextUtils
    {
        public static T ParseBody<T>(this IHttpContext context)
        {
            var body = context.Request.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static object ParseBody(this IHttpContext context, Type type)
        {
            var body = context.Request.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();

            return JsonConvert.DeserializeObject(text, type);
        }

        public static JToken ParseJson(this IHttpContext context)
        {
            var body = context.Request.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JToken.Parse(text);
        }

        public static Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, object jsonBody)
        {
            var jsonText = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
            return HTTPContextUtils.SendResponse(context, statusCode, "application/json", jsonText);
        }

        public static async Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, string contentType = null, string body = null)
        {
            var response = context.Response;

            response.StatusCode = statusCode;

            if (contentType != null)
            {
                response.Headers.Add("Content-Type", contentType);
            }

            if (contentType == "application/json")
            {
                await response.WriteAllJsonAsync(body);
            }
            else if (body != null)
            {
                // This is resulting in no data sent.
                //  What is WriteAllJsonAsync actually doing that makes it json specific?
                // var stream = new MemoryStream();
                // var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
                // writer.Write(body);
                // await response.WriteAllAsync(stream, contentType);
                await response.WriteAllJsonAsync(body);
            }
        }
    }
}