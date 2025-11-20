using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Domain.Ports.Services;
using Lab11_Juli.Infrastructure.Adapters.Respositories;
using Lab11_Juli.Infrastructure.Adapters.Security;
using Lab11_Juli.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab11_Juli.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Conexión a la Base de Datos
        services.AddDbContext<TicketerabdContext>((serviceProvider, options) =>
        {
            // PASO A: Intentar leer la variable de entorno del sistema operativo (Render)
            // Usamos una variable simple sin jerarquías complejas
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            // PASO B: Si está vacía (significa que estamos en Local), leemos del appsettings.json
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            // Opcional: Imprimir en consola para depurar si falla (solo verás esto en los logs de Render)
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("ERROR CRÍTICO: No se encontró ninguna cadena de conexión.");
            }

            options.UseNpgsql(connectionString);
        });
        //unitofwork
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
    
    

}