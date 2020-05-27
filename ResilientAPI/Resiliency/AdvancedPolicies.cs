using Polly;
using ResilientAPI.Resiliency.Simple;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ResilientAPI.Resiliency
{
    public static class AdvancedPolicies
    {
        private static HttpStatusCode[] _statusCodes =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public static IAsyncPolicy<HttpResponseMessage> GetWrappedAysncPolicy() =>
            SimplePolicies.GetRetryPolicy()
                .WrapAsync(SimplePolicies.GetCircuitBreakerPolicy())
                .WrapAsync(SimplePolicies.GetTimeoutPolicy());

        public static IAsyncPolicy<HttpResponseMessage> GetAdvancedCircuitBreakerPolicy() => Policy.
            Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(response => _statusCodes.Contains(response.StatusCode))
            .AdvancedCircuitBreakerAsync(
                failureThreshold: .5,
                samplingDuration: TimeSpan.FromSeconds(5),
                minimumThroughput: 5,
                durationOfBreak: TimeSpan.FromSeconds(5)
            );
     }
}
