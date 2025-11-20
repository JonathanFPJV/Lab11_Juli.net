# Etapa 1: Construcción (Usamos la imagen oficial de .NET 9 SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar el archivo de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto de los archivos y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Ejecución (Usamos la imagen ligera de .NET 9 Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Configurar el puerto que usa Render (Por defecto 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080


ENTRYPOINT ["dotnet", "Lab11_Juli.dll"]