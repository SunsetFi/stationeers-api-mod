namespace StationeersWebApi.Controllers
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using StationeersWebApi.Server;
    using StationeersWebApi.Server.Attributes;
    using StationeersWebApi.Server.Exceptions;

    /// <summary>
    /// A controller for providing hosted web content.
    /// </summary>
    [WebController(Path = "/")]
    public class WebhostController
    {
        /// <summary>
        /// Gets the favicon for hosted content.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that resolves when the request is completed.</returns>
        [WebRouteMethod(Method = "GET", Path = "favicon.png")]
        public async Task GetFavIcon(IHttpContext context)
        {
            // TODO: favicon
            await context.SendResponse(HttpStatusCode.NotFound, "text/plain", "Not Found");
        }

        /// <summary>
        /// Gets the index of the web host.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that is resolved when the request is completed.</returns>
        [WebRouteMethod(Method = "GET")]
        public Task GetIndex(IHttpContext context)
        {
            return this.SendDirectoryContent(context, StationeersWebApiPlugin.WebhostPath);
        }

        /// <summary>
        /// Gets the content of the given path from the web host.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="path">The request path.</param>
        /// <returns>A task that is resolved when the request is completed.</returns>
        [WebRouteMethod(Method = "GET", Path = "**path", Priority = -1000)]
        public Task GetPath(IHttpContext context, string path)
        {
            // This is a bit hackish, but we dont want to give false results for anything that is actually our api.
            // Might want to find a more elegant solution here, as this isnt pluggable by other plugins.
            if (path.StartsWith("api"))
            {
                throw new NotFoundException();
            }

            path = this.NormalizeValidatePath(path);
            if (File.Exists(path))
            {
                return this.SendFile(context, path);
            }
            else if (Directory.Exists(path))
            {
                return this.SendDirectoryContent(context, path);
            }
            else
            {
                throw new NotFoundException();
            }
        }

        private Task SendFile(IHttpContext context, string path)
        {
            return context.SendFileResponse(path);
        }

        private Task SendDirectoryContent(IHttpContext context, string path)
        {
            if (File.Exists(Path.Combine(path, "index.html")))
            {
                return this.SendFile(context, Path.Combine(path, "index.html"));
            }

            if (File.Exists(Path.Combine(path, "index.htm")))
            {
                return this.SendFile(context, Path.Combine(path, "index.htm"));
            }

            var body = new StringBuilder();
            body.Append("""<html><body><ul>""");

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var name = Path.GetFileName(directory);
                body.AppendLine($"<li><a href=\"{name}/\">{name}</a></li>");
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                body.AppendLine($"<li><a href=\"{name}\">{name}</a></li>");
            }

            body.AppendLine("</ul></body></html>");

            return context.SendResponse(HttpStatusCode.OK, "text/html", body.ToString());
        }

        private string NormalizeValidatePath(string path)
        {
            if (path == null)
            {
                throw new NotFoundException();
            }

            var webhostPath = Path.GetFullPath(StationeersWebApiPlugin.WebhostPath);
            if (!webhostPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                webhostPath += Path.DirectorySeparatorChar;
            }

            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                path = path.Substring(1);
            }

            path = Path.GetFullPath(Path.Combine(webhostPath, path));

            if (!path.StartsWith(webhostPath))
            {
                throw new NotFoundException();
            }

            return path;
        }
    }
}
