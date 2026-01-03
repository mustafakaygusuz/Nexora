
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nexora.Api.OperationFilters;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.JsonConverters;
using Nexora.Core.Contexts;
using Nexora.Core.StartupExtensions;
using Nexora.Core.Validation.Extensions;
using Nexora.Data.ConsumersRepositories;
using Nexora.Data.Domain.DbContexts;
using Nexora.Data.SystemConfigurationManagers;
using Nexora.Security.JWT;
using Nexora.Services.AuthServices.Dtos.Validators;
using Nexora.Services.Common.Middlewares;
using Nexora.Services.ConsumersServices;
using System.ComponentModel;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables();

SetupAuthentication();
SetupDI();
SetupApiVersioning();
SetupSwagger();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.Converters.Add(new EncryptIdConverterFactory(builder.Configuration.Get<EncryptionConfigurationModel>()!));
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");
//app.UseMiddleware<SetContextMiddleware>();
//app.UseMiddleware<JwtAuthenticationMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<DecryptIdParametersMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SetupDI()
{
    builder.Services.AddScoped<ApiContext>();

    builder.Services.AddDatabase<ApplicationDbContext>(builder.Configuration);

    builder.Services.AddDapper();

    builder.Services.BindOptions(builder.Configuration);

    builder.Services.AddCacheWithStackExchange(builder.Configuration);

    builder.Services.AddHttpClientWithSSL();

    builder.Services.AddEncryption(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "CorsPolicy",
            corsPolicyBuilder =>
            {
                corsPolicyBuilder
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin =>
                  Uri.TryCreate(origin, UriKind.Absolute, out var validatedUri) &&
                  (
                    validatedUri.Host.EndsWith(".kaizenloyalty.com") ||
                    (!builder.Environment.IsProduction() && validatedUri.Host.Equals("localhost"))))
                  .WithMethods("GET", "POST", "DELETE");
            });
    });

    builder.Services.Scan(scan => scan
      .FromAssemblyOf<IConsumersService>()
        .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

    builder.Services.Scan(scan => scan
        .FromAssemblyOf<IConsumersRepository>()
          .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
          .AsImplementedInterfaces()
          .WithScopedLifetime());

    builder.Services.Scan(scan => scan
        .FromAssemblyOf<SystemConfigurationManager>()
          .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Manager")))
          .AsImplementedInterfaces()
          .WithScopedLifetime());

    builder.Services.AddScoped<ITokenHelper>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>().GetSection("Jwt");
        var secret = config.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Jwt:SecretKey not configured");
        var issuer = config.GetValue<string>("Issuer") ?? "nexora";
        var audience = config.GetValue<string>("Audience") ?? "nexora";
        var accessMinutes = config.GetValue<int?>("AccessTokenMinutes") ?? 60;
        var refreshDays = config.GetValue<int?>("RefreshTokenDays") ?? 1;

        return new JwtTokenHelper(secret, issuer, audience, accessMinutes, refreshDays);
    });
    builder.Services.AutoAddValidators<AuthRegisterRequestValidator>();
    //MappingConfig.Configure();
}

void SetupApiVersioning()
{
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    });
}

void SetupAuthentication()
{
    builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = _ => Task.CompletedTask,
            OnTokenValidated = _ => Task.CompletedTask,
            OnAuthenticationFailed = _ => Task.CompletedTask,
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
        };
    });

    builder.Services.AddAuthorization();
    builder.Services.AddEndpointsApiExplorer();
}

void SetupSwagger()
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "bearer",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new List<string>()
            }
        });

        options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

        options.OperationFilter<RequiredHeaderParameterFilter>();

        options.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);
    });
}