using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using ResilientAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
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

        public async Task<HttpResponseMessage> CallUnreliableEndpoint(GenerateResponseRequestWaitWhat request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("unreliable-endpoints", content);
        }
    }
}
