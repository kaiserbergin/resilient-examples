using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ResilientAPI.Models;
using System.Threading.Tasks;
using ResilientAPI.Resiliency.Simple;
using ResilientAPI.Clients;
using System.Threading;

namespace ResilientAPI.Controllers
{
    [Route("policies")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly UnreliableEndpointsClient _unPoliciedClient;
        private readonly UnreliableEndpointsClientPartDuex _policiedClient;
        private readonly UnreliableForAdvancedCircuitBreaker _advancedCircuitBreakerClient;

        public PolicyController(
            UnreliableEndpointsClient unreliableEndpointsClient, 
            UnreliableEndpointsClientPartDuex unreliableEndpointsClientPartDuex,
            UnreliableForAdvancedCircuitBreaker advancedCircuitBreakerClient)
        {
            _unPoliciedClient = unreliableEndpointsClient;
            _policiedClient = unreliableEndpointsClientPartDuex;
            _advancedCircuitBreakerClient = advancedCircuitBreakerClient;
        }
        
        [HttpPost]
        [Route("single-policy")]
        public async Task<HttpResponseMessage> ExecuteSinglePolicy(PolicyExecutionRequest request)
        {
            var executionResult = request.PolicyExecution switch
            {
                PolicyExecutionType.Timeout => await SimplePolicies.GetTimeoutPolicy()
                    .ExecuteAsync(async cancellationToken => 
                        await _unPoliciedClient.CallUnreliableEndpoint(request.InnerHttpCall, cancellationToken),
                        CancellationToken.None),

                PolicyExecutionType.Retry => await SimplePolicies.GetRetryPolicy()
                    .ExecuteAsync(async () => await _unPoliciedClient.CallUnreliableEndpoint(request.InnerHttpCall)),

                PolicyExecutionType.CircuitBreaker => await SimplePolicies.GetCircuitBreakerPolicy()
                    .ExecuteAsync(async () => await _unPoliciedClient.CallUnreliableEndpoint(request.InnerHttpCall)),

                _ => throw new NotImplementedException()
            };

            return executionResult;
        }

        [HttpPost]
        [Route("wrapped-policy")]
        public async Task<HttpResponseMessage> ExecuteWrappedPolicy(GenerateResponseRequestWaitWhat request) => 
            await _policiedClient.CallUnreliableEndpoint(request);

        [HttpPost]
        [Route("advanced-circuit-breaker")]
        public async Task<HttpResponseMessage> ExecuteAdvancedCircuitBreaker(GenerateResponseRequestWaitWhat request) =>
            await _advancedCircuitBreakerClient.CallUnreliableEndpoint(request);
    }
}
