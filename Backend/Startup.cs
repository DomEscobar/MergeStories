using API.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PublicTimeAPI.Filters;
using PublicTimeAPI.Repository;
using System;
using System.IO;
using WebApi.Extensions.Configuration;
using WebApi.Extensions.DbContext;

namespace PublicTimeAPI
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

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContextPool<ApplicationDbContext>(o => o.Configure(Configuration));

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
      });

      // IP rate limit
      services.AddOptions();
      services.AddMemoryCache();
      services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
      services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
      services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
      services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


      services.AddResponseCompression(options =>
      {
        options.Providers.Add<GzipCompressionProvider>();
      });

      services.AddHealthChecks();
      services.AddHttpContextAccessor();
      services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
      services.AddTransient<IRequestService, RequestService>();
      services.AddTransient<IDatetimeprovider, Datetimeprovider>();

      services.AddAuthentication(o =>
      {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(o =>
      {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = false,
          ValidateAudience = false,
        };
      });

      services.AddMvc(config =>
      {
        config.Filters.Add(typeof(CustomExceptionFilterAttribute));
        config.EnableEndpointRouting = false;
      }).AddNewtonsoftJson(o =>
      {
        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      });
    }

    public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, ApplicationDbContext context)
    {
      bool isInMemory = Configuration.IsInMemoryFallback();

      if (!isInMemory)
      {
        context.Database.Migrate();
      }
      else
      {
        context.Database.EnsureCreated();
      }

      app.Use(async (cotxt, next) =>
      {
        await next();

        if (cotxt.Response.StatusCode == 404 &&
            !Path.HasExtension(cotxt.Request.Path.Value) &&
            !cotxt.Request.Path.Value.StartsWith("/api/"))
        {
          cotxt.Request.Path = "/index.html";
          await next();
        }
      });

      app.UseIpRateLimiting();

      app.UseAuthentication();
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseHealthChecks("/hc");
      app.UseResponseCompression();

      app.UseCors("CorsPolicy");

      app.UseMvc();
    }
  }
}
