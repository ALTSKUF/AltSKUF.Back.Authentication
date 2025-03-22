using AltSKUF.Back.Authentication.Domain.Scheduler.Jobs;
using AltSKUF.Back.Authentication.Domain.Scheduler;
using AltSKUF.Back.Authentication.Domain.Services;
using AltSKUF.Back.Authentication.Domain.Services.Runtime;
using Quartz.Spi;
using AltSKUF.Back.Authentication.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using AltSKUF.Back.Authentication.Domain.Extensions;
using AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users;
using AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users.Runtime;

namespace AltSKUF.Back.Authentication.Extensions
{
    public static class ServiceExtensions
    {
        public static void UseServiceExtensions(this WebApplicationBuilder builder)
        {
            builder.ReadConfiguration();
            builder.AddServices();
            builder.AddHttpClient();
            builder.AddScheduler();
            builder.AddAuth();

            builder.AddSwagger();
        }

        private static void ReadConfiguration(this WebApplicationBuilder builder)
        {
            Configuration.Singleton = builder.Configuration
                .GetSection("DefaultConfiguration")
                .Get<Configuration>() ?? new();
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthenticationService, AuthneticationService>();
        }

        private static void AddHttpClient(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService, UserService>(_ =>
                new(new()
                {
                    BaseAddress = new(Configuration.Singleton.UserServiceUrl)
                }));
        }

        private static void AddScheduler(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<AccessSecretJob>();
            builder.Services.AddTransient<RefreshSercretJob>();
            builder.Services.AddScoped<IJobFactory, JobFactory>();
        }

        private static void AddAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddJwtBearer("Services", _ =>
                {
                    _.Audience = "AltSkuf";
                    _.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "AltSKUF.Back",
                        ValidateAudience = true,
                        ValidAudience = "AltSKUF.Back",
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.Singleton.ServicesSercret)),
                        ValidateIssuerSigningKey = true,
                    };
                })
                .AddJwtBearer("Refresh", _ =>
                {
                    _.Audience = "AltSkuf";
                    _.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "AltSKUF.Back",
                        ValidateAudience = true,
                        ValidAudience = "AltSKUF.Front",
                        ValidateLifetime = true,
                        IssuerSigningKeyResolver =
                            (token, secutiryToken, kid, validationParameters) =>
                                [SecretExtensions.RefreshTokenSecret,
                                 SecretExtensions.PreviousRefreshTokenSecret],
                        ValidateIssuerSigningKey = true,
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Services", policy =>
                {
                    policy.AddAuthenticationSchemes("Services");
                    policy.RequireAuthenticatedUser();
                })
                .AddPolicy("Refresh", policy =>
                {
                    policy.AddAuthenticationSchemes("Refresh");
                    policy.RequireAuthenticatedUser();
                });
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Add JWT authorization support
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // Must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });
            });
        }
    }
}
