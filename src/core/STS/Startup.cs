using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STS.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Services.Default;

namespace STS
{
    public class Startup
    {
        private const string CORS_POLICY_NAME = "allowAll";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure CORS
            ConfigureCors(services);

            // Configure security related things
            ConfigureSecurity(services);

            // Configure Web API
            ConfigureMvc(services);
        }

        private void ConfigureCors(IServiceCollection services)
        {
            // For this demo allow everything so we don't have to hastle around
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY_NAME, cors =>
                {
                    cors.AllowAnyHeader();
                    cors.AllowAnyMethod();
                    cors.AllowAnyOrigin();
                    cors.AllowCredentials();
                });
            });
        }

        private void ConfigureSecurity(IServiceCollection services)
        {
            var identityServer = services.AddIdentityServer(options =>
            {
                // Use ONLY for developing! Never use this in production. Never.
                options.IssuerUri = "http://localhost:5001/";
            });

            //needed?
            identityServer.AddInMemoryStores();

            identityServer.AddInMemoryClients(Clients.Get());
            identityServer.AddInMemoryScopes(Scopes.Get());
            identityServer.AddInMemoryUsers(Users.Get());

            // Enable CORS on identity server
            identityServer.Services.AddTransient<ICorsPolicyService>(p => {
                var corsService = new DefaultCorsPolicyService(p.GetRequiredService<ILogger<DefaultCorsPolicyService>>());
                corsService.AllowAll = true;
                return corsService;
            });
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            var mvcCore = services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseCors(CORS_POLICY_NAME);

            app.UseIdentityServer();

            app.UseMvc();

            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });*/
        }
    }
}
