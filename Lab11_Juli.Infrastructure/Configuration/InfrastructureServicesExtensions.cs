using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Domain.Ports.Services;
using Lab11_Juli.Infrastructure.Adapters.Respositories;
using Lab11_Juli.Infrastructure.Adapters.Security;
using Lab11_Juli.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lab11_Juli.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Conexión a la Base de Datos
        services.AddDbContext<TicketerabdContext>((serviceProvider, options) =>
        {
            string? connectionString = null;

            // 1. Intentar leer variable de entorno 'DB_CONNECTION' (Configurada en Render)
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            if (!string.IsNullOrEmpty(envConnectionString))
            {
                // ESTAMOS EN RENDER (PRODUCCIÓN)
                try 
                {
                    // Render nos da una URL tipo: postgres://user:pass@host/db
                    // Npgsql necesita: Host=...;Username=...;Password=...
                    var databaseUri = new Uri(envConnectionString);
                    var userInfo = databaseUri.UserInfo.Split(':');

                    var builder = new NpgsqlConnectionStringBuilder
                    {
                        Host = databaseUri.Host,
                        Port = databaseUri.Port,
                        Username = userInfo[0],
                        Password = userInfo[1],
                        Database = databaseUri.LocalPath.TrimStart('/'),
                        
                        // --- CONFIGURACIÓN CRÍTICA PARA RENDER ---
                        SslMode = SslMode.Require, 
                        TrustServerCertificate = true // Evita el error "The remote certificate was rejected"
                    };

                    connectionString = builder.ToString();
                    Console.WriteLine($"✅ [Render] Conectando a BD Externa: {databaseUri.Host}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [Error] Falló el parseo de la URL de Render: {ex.Message}");
                }
            }
            else
            {
                // ESTAMOS EN LOCAL (TU PC)
                // Lee del appsettings.json
                connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine("💻 [Local] Usando conexión de appsettings.json");
            }

            // Conectar
            options.UseNpgsql(connectionString);
        });
        //unitofwork
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
    
    

}