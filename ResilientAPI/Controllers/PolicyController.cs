using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ResilientAPI.Models;
using System.Threading.Tasks;
using Polly;
using ResilientAPI.Resiliency.Simple;

namespace ResilientAPI.Controllers
{
    [Route("policies")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PolicyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BASE_URL"));
        }
        
        [HttpPost]
        [Route("single-policy")]
        public async Task<IActionResult> ExecuteSinglePolicy(PolicyExecutionRequest request)
        {
            
            return Ok();
        }
    }
}
