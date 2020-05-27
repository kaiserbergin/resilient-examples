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

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryWaitInMilliseconds = 100) => Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(response => _statusCodes.Contains(response.StatusCode))
            .WaitAndRetryAsync(
                retryCount: 1,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(retryWaitInMilliseconds)
            );

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int timeSpanInMilliseconds = 10000) => Policy.
            Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(response => _statusCodes.Contains(response.StatusCode))
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMilliseconds(timeSpanInMilliseconds)
            );

        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutInMilliseconds = 250) =>
            Policy.TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromMilliseconds(timeoutInMilliseconds),
                timeoutStrategy: TimeoutStrategy.Optimistic
            );
    }
}
