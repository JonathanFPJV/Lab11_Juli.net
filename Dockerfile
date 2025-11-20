# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos TODO el contenido del repositorio al contenedor
# Esto es vital para que encuentre Domain, Infrastructure y Application
COPY . .

# Restauramos dependencias apuntando específicamente a tu proyecto principal
# Ajusta "Lab11_Juli/Lab11_Juli.csproj" si la carpeta se llama diferente,
# pero basándome en tu imagen, parece ser esa la ruta.
RUN dotnet restore "Lab11_Juli/Lab11_Juli.csproj"

# Publicamos el proyecto principal
RUN dotnet publish "Lab11_Juli/Lab11_Juli.csproj" -c Release -o /app/out

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Configuración de puertos para Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Lab11_Juli.dll"]