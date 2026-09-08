using System.Text.Json;
using System.Text.Json.Serialization;
using DPBack.API.Extensions;
using DPBack.API.Middleware;
using DPBack.Application.Abstractions;
using DPBack.Infrastructure.Payments;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.AddOptions(configuration);
builder.Services.AddCorsPolicy(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "Jwt",
            Description = "Enter jwt",
            In=ParameterLocation.Header
        });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDatabase(configuration);
builder.Services.AddApplicationServices();
builder.Services.AddBackgroundServices();
builder.Services.AddAuthorizationServices(configuration);
builder.Services.AddHttpClient<IPaymentService, PayUService>(client =>
{
    client.BaseAddress = new Uri(configuration["PayU:BaseAddress"]!);
});
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
});
QuestPDF.Settings.License = LicenseType.Community;
var app = builder.Build();
await app.SeedDBAsync(configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});
app.UseCors();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();