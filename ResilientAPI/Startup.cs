using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResilientAPI.Clients;
using ResilientAPI.Resiliency;
using ResilientAPI.Constants;
using Prometheus;
using ResilientAPI.HealthChecks;

namespace ResilientAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            #region Resiliency

            services.AddPolicyRegistry(Registry.GetRegistry());

            services.AddHttpClient<UnreliableEndpointsClient>();            
            services.AddHttpClient<UnreliableEndpointsClientPartDuex>()
                .AddPolicyHandlerFromRegistry(PolicyConstants.COMBO_POLICY_NAME);
            services.AddHttpClient<UnreliableForAdvancedCircuitBreaker>()
                .AddPolicyHandlerFromRegistry(PolicyConstants.ADVANCED_CIRCUITBREAKER_POLICY_NAME);

            #endregion

            #region Metrics

            services.AddSingleton<IGauge>(Metrics.CreateGauge("queued_work", "So much to do and so little time."));
            services.AddSingleton<ICounter>(Metrics.CreateCounter("work_done", "we work hard for the money."));

            #endregion

            #region Health Checks

            services.AddHealthChecks()
                .AddCheck<WorkHealth>("work-health")
                .AddCheck<AdvancedCircuitBreakerHealth>("circuit-breaker-health");

            #endregion


            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseMetricServer(url: "/metrics");

        }
    }
}
