using Polly.Registry;
using ResilientAPI.Constants;
using ResilientAPI.Resiliency.Simple;

namespace ResilientAPI.Resiliency
{
    public static class Registry
    {
        public static PolicyRegistry GetRegistry() =>
            new PolicyRegistry()
            {
                { PolicyConstants.TIMEOUT_POLICY_NAME, SimplePolicies.GetTimeoutPolicy() },
                { PolicyConstants.RETRY_POLICY_NAME, SimplePolicies.GetRetryPolicy() },
                { PolicyConstants.CIRCUITBREAKER_POLICY_NAME, SimplePolicies.GetCircuitBreakerPolicy() },
                { PolicyConstants.COMBO_POLICY_NAME, AdvancedPolicies.GetWrappedAysncPolicy() }
            };
    }
}
