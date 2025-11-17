using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Extensions
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["AppSettings:Token"]!)),
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role,
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Token validation failed: {context.Exception.Message}");
                        Console.ResetColor();
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✅ Token successfully validated.");
                        Console.ResetColor();
                        return Task.CompletedTask;
                    }
                };

            });
            return services;
        }
    }
}
