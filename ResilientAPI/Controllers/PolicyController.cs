using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ResilientAPI.Models;
using System.Threading.Tasks;
using Polly;
using ResilientAPI.Resiliency.Simple;
using ResilientAPI.Clients;

namespace ResilientAPI.Controllers
{
    [Route("policies")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly UnerliableEndpointsClient _httpClient;

        public PolicyController(UnerliableEndpointsClient httpClient)
        {
            _httpClient = httpClient; 
        }
        
        [HttpPost]
        [Route("single-policy")]
        public async Task<IActionResult> ExecuteSinglePolicy(PolicyExecutionRequest request)
        {
            var executionResult = request.PolicyExecution switch
            {
                PolicyExecutionType.Timeout => await SimplePolicies.GetTimeoutPolicy()
                    .ExecuteAsync(async () => await _httpClient.CallUnreliableEndpoint(request.InnerHttpCall)),

                PolicyExecutionType.Retry => await SimplePolicies.GetRetryPolicy()
                    .ExecuteAsync(async () => await _httpClient.CallUnreliableEndpoint(request.InnerHttpCall)),

                PolicyExecutionType.CircuitBreaker => await SimplePolicies.GetCircuitBreakerPolicy()
                    .ExecuteAsync(async () => await _httpClient.CallUnreliableEndpoint(request.InnerHttpCall)),

                _ => throw new NotImplementedException()
            };

            return Ok(executionResult);
        }
    }
}
