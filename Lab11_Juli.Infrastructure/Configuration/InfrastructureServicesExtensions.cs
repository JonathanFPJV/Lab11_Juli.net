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
            // 1. Intentamos leer la variable de entorno DIRECTAMENTE (Bypass de IConfiguration)
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            // 2. Si está vacía, intentamos leer la variable estándar de Render (a veces Render usa esta)
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            }

            // 3. Si sigue vacía, usamos la del archivo JSON (Solo para Local)
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("⚠️ ALERTA: Usando configuración local (appsettings.json)");
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            else 
            {
                Console.WriteLine("✅ ÉXITO: Usando configuración de Variable de Entorno");
                
                // PARSEO IMPORTANTE:
                // Si la URL viene con formato 'postgres://' (Render), a veces Npgsql la prefiere convertida.
                // Este bloque convierte la URL de Render a un formato de conexión estándar de ADO.NET
                try 
                {
                    var databaseUri = new Uri(connectionString);
                    if (databaseUri.Scheme == "postgres") // Solo si es formato URL
                    {
                        var userInfo = databaseUri.UserInfo.Split(':');
                        var builder = new NpgsqlConnectionStringBuilder
                        {
                            Host = databaseUri.Host,
                            Port = databaseUri.Port,
                            Username = userInfo[0],
                            Password = userInfo[1],
                            Database = databaseUri.LocalPath.TrimStart('/'),
                            SslMode = SslMode.Require, // Render exige SSL
                            TrustServerCertificate = true // Para evitar errores de certificados en la nube
                        };
                        connectionString = builder.ToString();
                        Console.WriteLine("🔄 URL convertida a ConnectionString exitosamente.");
                    }
                }
                catch (Exception ex)
                {
                    // Si falla el parseo, usamos el string original, quizás ya venía bien
                    Console.WriteLine($"⚠️ No se pudo parsear la URI, usando string original: {ex.Message}");
                }
            }

            // Log de seguridad (Muestra el Host pero oculta la contraseña)
            Console.WriteLine($"🔌 Intentando conectar a: {connectionString?.Split("Password")[0]}...");

            options.UseNpgsql(connectionString);
        });
        //unitofwork
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
    
    

}