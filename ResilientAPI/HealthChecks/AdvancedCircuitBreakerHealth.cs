using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly.CircuitBreaker;
using Polly.Registry;
using ResilientAPI.Constants;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientAPI.HealthChecks
{
    public class AdvancedCircuitBreakerHealth : IHealthCheck
    {
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public AdvancedCircuitBreakerHealth(IReadOnlyPolicyRegistry<string> registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            _circuitBreakerPolicy = registry.Get<AsyncCircuitBreakerPolicy<HttpResponseMessage>>(PolicyConstants.ADVANCED_CIRCUITBREAKER_POLICY_NAME);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_circuitBreakerPolicy.CircuitState == CircuitState.Open)
                return Task.FromResult(HealthCheckResult.Degraded("circuit breaker tripped"));

            return Task.FromResult(HealthCheckResult.Healthy("we good"));
        }
    }
}
