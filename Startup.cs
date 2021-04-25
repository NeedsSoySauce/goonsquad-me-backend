using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NeedsSoySauce.Authorization;
using NeedsSoySauce.Data;
using NeedsSoySauce.Repositories;
using NeedsSoySauce.Services;
using NeedsSoySauce.Services.RawgApiGamesService;
using NeedsSoySauce.SignalR;

namespace NeedsSoySauce
{
    public class Startup
    {
        readonly string CorsPolicy = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(CorsPolicy, builder => builder.WithOrigins("http://localhost:3000").AllowCredentials().WithMethods("GET", "POST").AllowAnyHeader()));

            services.AddTransient<IGamesService, RawgApiGamesService>(s => new RawgApiGamesService(Configuration["RawgApiKey"]));
            services.AddTransient<IGamesRepo, GamesRepo>();
            services.AddTransient<IGoonsquadsRepo, GoonsquadsRepo>();
            services.AddTransient<IGoonsRepo, GoonsRepo>();
            services.AddTransient<IJobsRepo, JobsRepo>();

            services.AddDbContext<ApplicationDbContext>(options => {
                // options.UseSqlite(@"Data Source=./goonsquad-me.db");
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));

            });

            string domain = $"https://{Configuration["Auth0:Domain"]}/";

            services.AddSingleton<Auth0ApiWrapper>(s => new Auth0ApiWrapper(
                clientId: Configuration["Auth0:ClientId"],
                clientSecret: Configuration["Auth0:ClientSecret"],
                domain: Configuration["Auth0:Domain"],
                managementApiAudience: Configuration["Auth0:ManagementApiAudience"]
            ));

            // Based on: https://auth0.com/docs/quickstart/backend/aspnet-core-webapi/01-authorization
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                    // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("profile", policy => policy.Requirements.Add(new HasScopeRequirement("profile", domain)));
                options.AddPolicy("email", policy => policy.Requirements.Add(new HasScopeRequirement("email", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddHostedService<MatchmakingService>();
            services.AddScoped<MatchmakingService.IScopedProcessingService, MatchmakingService.ScopedMatchmakingService>();

            // Based on: https://docs.microsoft.com/en-gb/azure/azure-signalr/signalr-quickstart-dotnet-core
            services.AddSignalR()
                    .AddAzureSignalR(Configuration.GetConnectionString("SignalR"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "goonsquad me", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "goonsquad me api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
