FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de solución y restaurar dependencias
COPY ["TDDProductoApi.sln","./"]
COPY ["./Presentation/Presentation.csproj", "Presentation/"]
COPY ["./Application/Application.csproj", "Application/"]
COPY ["./Domain/Domain.csproj", "Domain/"]
COPY ["./Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["./TDDProductoApi.Tests/TDDProductoApi.Tests.csproj", "TDDProductoApi.Tests/"]

RUN dotnet restore "TDDProductoApi.sln"

# Copiar el resto del código fuente y compilar la aplicación
COPY . .
WORKDIR /src/Presentation
RUN dotnet publish "Presentation.csproj" -c Release --no-restore -o /App/out

EXPOSE 8097

# Etapa final: Usamos el runtime de .NET para la imagen final
FROM base AS final
WORKDIR /App
COPY --from=build /App/out ./

ENTRYPOINT ["dotnet", "Presentation.dll"]
