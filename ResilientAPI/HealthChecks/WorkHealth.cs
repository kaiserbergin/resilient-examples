using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientAPI.HealthChecks
{
    public class WorkHealth : IHealthCheck
    {
        private readonly IGauge _queuedWork;
        public WorkHealth(IGauge queuedWork)
        {
            _queuedWork = queuedWork ?? throw new ArgumentNullException(nameof(queuedWork));
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_queuedWork.Value > 5)
                return Task.FromResult(HealthCheckResult.Degraded("oof, too much work"));

            return Task.FromResult(HealthCheckResult.Healthy("we good"));
        }
    }
}
