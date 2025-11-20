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
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            if (!string.IsNullOrEmpty(envConnectionString))
            {
                try 
                {
                    var databaseUri = new Uri(envConnectionString);
                    var userInfo = databaseUri.UserInfo.Split(':');

                    var builder = new NpgsqlConnectionStringBuilder
                    {
                        Host = databaseUri.Host,
                        // --- CORRECCIÓN AQUÍ ---
                        // Si el puerto es -1 (no viene en la URL), usamos el 5432 por defecto
                        Port = databaseUri.Port == -1 ? 5432 : databaseUri.Port,
                        
                        Username = userInfo[0],
                        Password = userInfo[1],
                        Database = databaseUri.LocalPath.TrimStart('/'),
                        SslMode = SslMode.Require, 
                        TrustServerCertificate = true
                    };

                    connectionString = builder.ToString();
                    Console.WriteLine($"✅ [Render] Conectando a Host: {builder.Host}, Puerto: {builder.Port}, BD: {builder.Database}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [Error Crítico] Falló el parseo de la URL: {ex.Message}");
                    // Si falla el parseo, no tenemos conexión válida, esto causará error más abajo,
                    // pero al menos ya sabemos por qué.
                }
            }
            else
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine("💻 [Local] Usando conexión de appsettings.json");
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