using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CoreWebApiTest.Middleware;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using CoreWebApiTest.Services;

namespace CoreWebApiTest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebEncoders();

            // Add framework services.
            var mvcCore = services.AddMvc();
            //mvcCore.AddApiExplorer();
            mvcCore.Services.AddAuthorization();
            //mvcCore.AddFormatterMappings();

            // Razor is only needed for token things. 
            //mvcCore.Services.AddRazorViewEngine();
            //mvcCore.Services.AddDataAnnotations();
            ////MvcJsonMvcCoreBuilderExtensions.AddJsonFormatters(mvcCore, options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());
            //mvcCore.AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());


            // For CORS
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });

            // Either use this or the other customer service by switching the comments
            services.AddSingleton<ICustomerService, InMemoryCustomerService>();
            //services.AddSingleton<ICustomerService, DatabaseCustomerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // For CORs
            app.UseCors("AllowAll");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            UseIdentityServerSecurity(app);

            app.UseMvc();

            //use the time logging middleware
            app.UseMiddleware<MyTimeLoggerMiddleware>();
        }

        private void UseIdentityServerSecurity(IApplicationBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            
            app.UseIdentityServerAuthentication( new IdentityServerAuthenticationOptions()
            {
                Authority = "http://localhost:5001/",
                ScopeName = "api",
                ScopeSecret = "apisecret",
                RequireHttpsMetadata = false,

                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
        }
    }
}
