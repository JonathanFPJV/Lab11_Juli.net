namespace Lab11_Juli.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApiServce(this IServiceCollection services, IConfiguration configuration)
    {
        // Habilitar controladores de la API
        services.AddControllers();
        // Registra HttpContextAccessor (común para obtener info del request)
        services.AddHttpContextAccessor();
        
        return services;
    }
}