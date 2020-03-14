using System.IO;
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json;

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
        public static Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, object jsonBody)
        {
            var jsonText = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
            return HTTPContextUtils.SendResponse(context, statusCode, jsonText);
        }

        public static async Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, string jsonBody = null)
        {
            var response = context.Response;

            response.StatusCode = statusCode;

            response.Headers.Add("Content-Type", "application/json");
            if (jsonBody != null)
            {
                await response.WriteAllJsonAsync(jsonBody);
            }
        }
    }
}