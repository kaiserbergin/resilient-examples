using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Polly;
using ResilientAPI.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientAPI.Clients
{
    public class UnreliableEndpointsClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor contextAccessor;

        public UnreliableEndpointsClient(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            this.contextAccessor = contextAccessor;
        }
        public async Task<HttpResponseMessage> CallUnreliableEndpoint(GenerateResponseRequestWaitWhat request, CancellationToken cancellationToken = default)
        {
            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri($"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}");   

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("unreliable-endpoints", content, cancellationToken);
        }

    }
}
