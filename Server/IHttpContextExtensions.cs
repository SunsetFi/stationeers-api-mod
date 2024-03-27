namespace StationeersWebApi.Server
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Extension methods for the IHTTPContext interface.
    /// </summary>
    public static class IHttpContextExtensions
    {
        /// <summary>
        /// Parse the body of the request as a JSON object and deserialize it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the body to.</typeparam>
        /// <param name="context">The context to deserialize.</param>
        /// <returns>The body of the request deserialized as json.</returns>
        public static T ParseBody<T>(this IHttpContext context)
        {
            var reader = new StreamReader(context.Body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// Parse the body of the request as a JSON object and deserialize it to the specified type.
        /// </summary>
        /// <param name="context">The context to deserialize.</param>
        /// <param name="type">The type to which the body should be deserialized.</param>
        /// <returns>The body of the request deserialized to the specified type.</returns>
        public static object ParseBody(this IHttpContext context, Type type)
        {
            var reader = new StreamReader(context.Body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();

            return JsonConvert.DeserializeObject(text, type);
        }

        /// <summary>
        /// Parse the body of the request and return it as a JSON token.
        /// </summary>
        /// <param name="context">The context from which to parse the body.</param>
        /// <returns>The body of the request as a JToken.</returns>
        public static JToken ParseJson(this IHttpContext context)
        {
            var reader = new StreamReader(context.Body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JToken.Parse(text);
        }

        /// <summary>
        /// Send a response with the specified status code and body.
        /// </summary>
        /// <param name="context">The context to which to send the response.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="body">The body to send.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, string contentType, string body)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(body);
                writer.Flush();
                stream.Position = 0;
                await context.SendResponse(statusCode, contentType, stream);
            }
        }

        /// <summary>
        /// Send a response with the specified status code and a JSON body.
        /// </summary>
        /// <param name="context">The context to which to send the response.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        /// <param name="jsonBody">The object to serialize as JSON and include in the body of the response.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, object jsonBody)
        {
            var jsonText = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
            await SendResponse(context, statusCode, "application/json", jsonText);
        }

        /// <summary>
        /// Attempts to send the response.
        /// </summary>
        /// <param name="context">The context to which to send the response.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        /// <param name="jsonBody">The object to serialize as JSON and include in the body of the response.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task<bool> TrySendResponse(this IHttpContext context, HttpStatusCode statusCode, object jsonBody)
        {
            try
            {
                await SendResponse(context, statusCode, jsonBody);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends a file by path.
        /// </summary>
        /// <param name="context">The HTTP context of the request.</param>
        /// <param name="path">The path of the file to send.</param>
        /// <returns>A task that resolves when the request is completed.</returns>
        public static async Task SendFileResponse(this IHttpContext context, string path)
        {
            var mimeType = MimeMapper.GetMimeType(Path.GetExtension(path));
            await context.SendResponse(HttpStatusCode.OK, mimeType, new MemoryStream(File.ReadAllBytes(path)));
        }
    }
}
