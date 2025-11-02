using Microsoft.OpenApi.Models;

namespace Lab11_Juli.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Habilitar controladores de la API
        services.AddControllers();
        // Registra HttpContextAccessor (común para obtener info del request)
        services.AddHttpContextAccessor();
        // Habilitar Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mi API (Lab10_Juli)",
                Version = "v1",
                Description = "API para gestionar recursos del laboratorio."
            });
            
            // --- 2. CONFIGURACIÓN DE SWAGGER PARA JWT ---
            // Esto define el esquema de seguridad (Bearer token)
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Ingrese el token JWT así: Bearer {su_token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http, // También puede ser 'ApiKey' si prefieres
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            
            // Esto le dice a Swagger que aplique ese esquema a todas las operaciones
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
        return services;
    }
}