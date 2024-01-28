using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using StackExchange.Redis.Extensions.Core.Configuration;
using WebAPI.Endpoints;
using WebAPI.Extensions;
using WebAPI.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddResponseCompressionServices(); // From WebAPI\Extensions\ResponseCompressionExtensions.cs
builder.Services.AddHealthCheckServices(builder.Configuration); // From WebAPI\Extensions\HealthCheckExtensions.cs
builder.Services.AddJwtAuthenticationServices(builder.Configuration); // From WebAPI\Extensions\JwtBearerExtensions.cs

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenServices();

builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedis(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyPendingMigrations();
    app.GenerateSeedDataAndPersist();
}

app.UseCors();

app.UseHealthChecks("/_health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
); // RequireAuthorization, RequireCors, RequireHost possible for limiting calls for /_health endpoint.

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapApiEndpoints(); // From WebAPI\Extensions\EndpointExtensions.cs

app.Run();