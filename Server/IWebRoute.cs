using System.Threading.Tasks;

namespace WebAPI.Server
{
    public interface IWebRoute
    {
        string Method { get; }

        string Path { get; }

        Task OnRequested(IWebAPIContext context);
    }

}