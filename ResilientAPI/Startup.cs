using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResilientAPI.Clients;
using ResilientAPI.Resiliency;
using ResilientAPI.Constants;

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

            services.AddPolicyRegistry(Registry.GetRegistry());


            services.AddHttpClient<UnreliableEndpointsClient>();            
            services.AddHttpClient<UnreliableEndpointsClientPartDuex>()
                .AddPolicyHandlerFromRegistry(PolicyConstants.COMBO_POLICY_NAME);
            services.AddHttpClient<UnreliableForAdvancedCircuitBreaker>()
                .AddPolicyHandler(AdvancedPolicies.GetAdvancedCircuitBreakerPolicy());
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
            });
        }
    }
}
