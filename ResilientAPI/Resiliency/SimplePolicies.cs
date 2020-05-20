using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ResilientAPI.Resiliency.Simple
{
    public static class SimplePolicies
    {

        private static HttpStatusCode[] _statusCodes =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(response => _statusCodes.Contains(response.StatusCode))
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(100));

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(response => _statusCodes.Contains(response.StatusCode))
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(5));

        public static IAsyncPolicy GetTimeoutPolicy() => Policy.TimeoutAsync(5);
    }
    
}
