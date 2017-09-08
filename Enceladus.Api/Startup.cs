using Enceladus.Api.Data.EnceladusRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Enceladus.Api
{
    public class Startup
    {
        internal static IConfigurationRoot Configuration { get; private set; }
        internal static object localContextLockObj;
        internal static string EnvName;
        internal static bool StartupLogToFlatFile { get; private set; }
        public Startup(IHostingEnvironment env)
        {
            EnvName = env.EnvironmentName;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                Configuration = builder.Build();
                localContextLockObj = new object();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var scopePolicy = new AuthorizationPolicyBuilder()
              .RequireClaim("scope", "enceladusapi")
              .Build();
                // Add framework services.
                services.AddMemoryCache();
                services.AddMvc(options =>
                {

                   //options.Filters.Add(typeof(RequireHttpsAttribute));

                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.Authority = "https://enceladus-dev.auth0.com/";
                    options.Audience = "https://enceladus-dev.com/authorization";
                });
                services.AddLogging();
                services.AddDbContext<EnceladusContext>();
                services.AddScoped<IEnceladusRepository, EnceladusRepository>();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
                try
                {
                    app.UseCors(policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });



                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                /* app.UseJwtBearerAuthentication(new JwtBearerOptions()
                {
                    Audience = Startup.Configuration["AUTH0_CLIENT_ID"],
                    Authority = Startup.Configuration["AUTH0_DOMAIN"] + "/",
                    RequireHttpsMetadata = true,
                    Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            try
                            {
                                var claimsIdentity = context.Ticket.Principal.Identity as ClaimsIdentity;
                                if (claimsIdentity != null)
                                {
                                    string name = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                                }
                                return Task.FromResult(0);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            return null;
                        }
                    }

                });*/
                app.UseAuthentication();

                app.UseMvc();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

        }
    }
}
