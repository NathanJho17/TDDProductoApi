FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos
COPY TDDProductoApi.sln ./
COPY Presentation/Presentation.csproj Presentation/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY TDDProductoApi.Tests/TDDProductoApi.Tests.csproj TDDProductoApi.Tests/

# Restaurar paquetes
RUN dotnet restore TDDProductoApi.sln

# Copiar el resto del código fuente
COPY . .

# Publicar (IMPORTANTE: usar ruta explícita del .csproj y quitar --no-restore)
RUN dotnet publish Presentation/Presentation.csproj -c Release -o /App/out

# Imagen final con solo el runtime
FROM base AS final
WORKDIR /App
COPY --from=build /App/out ./

EXPOSE 8097
ENTRYPOINT ["dotnet", "Presentation.dll"]
