using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResilientAPI.Clients
{
    public class UnerliableEndpointsClient
    {
        private readonly HttpClient _httpClient;

        public UnerliableEndpointsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BASE_URL"));
        }
    }
}
