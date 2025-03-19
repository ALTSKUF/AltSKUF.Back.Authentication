using System.Text;
using AltSKUF.Back.Authentication.Domain;
using AltSKUF.Back.Authentication.Domain.Scheduler;
using AltSKUF.Back.Authentication.Domain.Scheduler.Jobs;
using AltSKUF.Back.Authentication.Domain.Services;
using AltSKUF.Back.Authentication.Domain.Services.Runtime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

Configuration.Singleton = builder.Configuration
    .GetSection("DefaultConfiguration")
    .Get<Configuration>() ?? new();

builder.Services.AddScoped<IAuthenticationService, AuthneticationService>();

builder.Services.AddTransient<AccessSecretJob>();
builder.Services.AddTransient<RefreshSercretJob>();
builder.Services.AddScoped<IJobFactory, JobFactory>();

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
                    [TokensSingleton.Singleton.RefreshTokenSecret, TokensSingleton.Singleton.PreviousRefreshTokenSecret],
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Services", policy =>
    {
        policy.AddAuthenticationSchemes("Services");
        policy.RequireAuthenticatedUser();
    });
    options.AddPolicy("Refresh", policy =>
    {
        policy.AddAuthenticationSchemes("Refresh");
        policy.RequireAuthenticatedUser();
    });
});

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

var scope = app.Services.CreateScope();
SecretScheduler.Start(scope.ServiceProvider).Wait();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();