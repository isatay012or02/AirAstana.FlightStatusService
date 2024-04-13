using System.Reflection;
using AirAstanaFlightStatusService.Api.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AirAstanaFlightStatusService.Api.Features.Swagger;

/// <summary>
/// Расширение сваггер для добавления в сборку
/// Setup and add swagger
/// </summary>
public static class SwaggerServiceExtensions
{
    /// <summary>
    /// Добавляет сваггер в сборку проекта
    /// Добавляет в контейнер зависимостей <see cref="SwaggerConfigureOptions"/>
    /// Добавляет xml для чтения комментарией для отображение в интерфейсе
    /// Добавляет поддержку Json
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();

        services.AddSwaggerGen(options =>
        {
            options.AddXmlComment(typeof(BaseController).Assembly);
            
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API для получению данных о статусе рейсов", Version = "v1" });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Заголовок авторизации с использованием схемы Bearer. Пример: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    /// <summary>
    /// Расширяет <see cref="SwaggerGenOptions"/> добавлением комментариев из xml
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assembly"></param>
    private static void AddXmlComment(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }
}