using Lab11_Juli.Configuration;
using Lab11_Juli.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 2. Registra servicios de la capa de Infraestructura (DbContext, Repositorios, UoW)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. Registra servicios de la capa de API (Controllers, Swagger, Auth, CORS)
builder.Services.AddApiServices(builder.Configuration);

// (La configuración de HttpJsonOptions está bien aquí)
builder.Services.ConfigureHttpJsonOptions(options =>
{
    //options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});


var app = builder.Build();

// --- Configuración del Pipeline (Middleware) ---


{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", name: "Lab11 - Juli V1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger abra en la raíz (midominio.com/)
    });
}

// app.UseCors("TuPoliticaDeCors"); // (Opcional)
// app.UseHttpsRedirection(); // (Recomendado)

// (Importante si usas JWT)
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

app.Run();
