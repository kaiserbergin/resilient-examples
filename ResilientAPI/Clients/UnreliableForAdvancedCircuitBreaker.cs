using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ResilientAPI.Clients
{
    public class UnreliableForAdvancedCircuitBreaker : UnreliableEndpointsClient
    {
        public UnreliableForAdvancedCircuitBreaker(HttpClient httpClient, IHttpContextAccessor contextAccessor) : base(httpClient, contextAccessor)
        {
        }
    }
}
