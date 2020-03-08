using System.Net;
using Newtonsoft.Json;

namespace WebAPI
{
    public static class ListenerContextUtils
    {
        public static void SendResponse(this HttpListenerContext context, int statusCode, object jsonBody)
        {
            var jsonText = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
            ListenerContextUtils.SendResponse(context, statusCode, jsonText);
        }

        public static void SendResponse(this HttpListenerContext context, int statusCode, string jsonBody)
        {
            var response = context.Response;

            response.StatusCode = statusCode;
            response.SendChunked = false;

            response.Headers.Add("Content-Type", "application/json");

            System.Text.Encoding encoding = response.ContentEncoding;
            if (encoding == null)
            {
                encoding = System.Text.Encoding.UTF8;
                response.ContentEncoding = encoding;
            }

            byte[] buffer = encoding.GetBytes(jsonBody);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            response.Close();
        }
    }
}