using System.Net;
using System.Text;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Identity.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace BaseProject.Infrastructure.Auth.Jwt;

public static class Startup
{
    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)));
        var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        if (string.IsNullOrEmpty(jwtSettings?.Key))
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtSettings:ValidIssiuer"],
                ValidAudience = config["JwtSettings:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow
            };
            options.Events = new JwtBearerEvents()
            {
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401;
                    context.ErrorDescription = "Authentication Failed.";
                    context.HandleResponse();
                    var payload = new JObject
                    {
                        ["errorCode"] = context.Response.StatusCode,
                        ["errorDescription"] = context.ErrorDescription,
                    };

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 401;

                    return context.Response.WriteAsync(payload.ToString());
                }
            };

        }).Services;

    }
}