using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ResilientAPI.Clients
{
    public class UnreliableEndpointsClientPartDuex : UnreliableEndpointsClient
    {
        public UnreliableEndpointsClientPartDuex(HttpClient httpClient, IHttpContextAccessor contextAccessor) : base(httpClient, contextAccessor) { }
    }
}
