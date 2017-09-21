using Enceladus.Api.Data.EnceladusRepository;
using Enceladus.Api.Helpers;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            localContextLockObj = new object();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                //var scopePolicy = new AuthorizationPolicyBuilder()
                //                   .RequireClaim("scope", new string[] { "openid" })
                //                  .Build();
                // Add framework services.

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("CustomAuthorization", policy => policy.Requirements.Add(new ScopeRequirement()));
                });
                services.AddMemoryCache();

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.Authority = "https://enceladus-dev.auth0.com/";
                    options.Audience = "https://api.enceladustechnologies.com";
                    options.IncludeErrorDetails = true;
                    
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            return Task.FromResult(0);
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.FromResult(0);
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.FromResult(0);
                        },
                        OnTokenValidated = context =>
                        {
                            try
                            {
                                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                                if (claimsIdentity != null)
                                {
                                    string name = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                                }
                                context.Success();
                                return Task.CompletedTask;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    };

                    options.Validate();
                });
                services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(RequireHttpsAttribute));
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
                services.AddLogging();
                services.AddDbContext<EnceladusContext>();
                services.AddScoped<IEnceladusRepository, EnceladusRepository>();
                services.AddSingleton<IAuthorizationHandler, EnceladusAuthorizationHandler>();
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

                app.UseStaticFiles();
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
