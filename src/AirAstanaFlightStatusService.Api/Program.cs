using System.Text;
using AirAstanaFlightStatusService.Api.Common.Options;
using AirAstanaFlightStatusService.Api.Features.Middlewares;
using AirAstanaFlightStatusService.Api.Features.Swagger;
using AirAstanaFlightStatusService.Api.Features.Versioning;
using AirAstanaFlightStatusService.Api.Features.WebApi;
using AirAstanaFlightStatusService.Application;
using AirAstanaFlightStatusService.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var webHostOptions = new WebHostOptions(
    instanceName: builder.Configuration.GetValue<string>($"{WebHostOptions.SectionName}:InstanceName"),
    webAddress: builder.Configuration.GetValue<string>($"{WebHostOptions.SectionName}:WebAddress"));

try
{
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);

    builder.ConfigureHostVersioning();

    builder.ConfigureWebHost();

    builder.Services.ConfigureApplicationAssemblies();

    builder.Services
        .ConfigureInfrastructurePersistence(builder.Configuration, builder.Environment.EnvironmentName)
        .ConfigureInfrastructureServices();
    
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("f2a1ed52710d4533bde25be6da03b6e3"))
            };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    Log.Information("{Msg} ({ApplicationName})...",
        "Запуск сборки проекта",
        webHostOptions.InstanceName);

    app.UseConfiguredSwagger();
    app.UseConfiguredVersioning();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionHandleMiddleware>();
    app.MapHealthChecks("/healthcheck");

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Программа была выброшена с исплючением ({ApplicationName})!",
        webHostOptions.InstanceName);
}
finally
{
    Log.Information("{Msg}!", "Высвобождение ресурсов логгирования");
    await Log.CloseAndFlushAsync();
}
