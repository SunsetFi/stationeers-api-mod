
using System.Net;

namespace StationeersWebApi.Authentication
{
    public static class EndpointUtils
    {
        public static string ToPortlessString(this EndPoint endpoint)
        {
            var ipEndpoint = endpoint as IPEndPoint;
            if (ipEndpoint != null)
            {
                return ipEndpoint.Address.ToString();
            }

            return endpoint.ToString();
        }
    }
}