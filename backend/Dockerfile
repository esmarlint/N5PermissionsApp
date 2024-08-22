# Establece la imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Establece la imagen para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos .csproj y restaura las dependencias
COPY ["N5PermissionsAPI/N5PermissionsAPI.Application.csproj", "N5PermissionsAPI/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["N5PermissionsAPI.Core/N5PermissionsAPI.Core.csproj", "N5PermissionsAPI.Core/"]
COPY ["N5PermissionsAPI.Infrastructure/N5PermissionsAPI.Infrastructure.csproj", "N5PermissionsAPI.Infrastructure/"]
COPY ["N5PermissionsAPI.Persistence/N5PermissionsAPI.Persistence.csproj", "N5PermissionsAPI.Persistence/"]

RUN dotnet restore "N5PermissionsAPI/N5PermissionsAPI.Application.csproj"

# Copia el resto del código fuente y compila
COPY . .
WORKDIR "/src/N5PermissionsAPI"
RUN dotnet build "N5PermissionsAPI.Application.csproj" -c Release -o /app/build

# Establece la imagen para publicar
FROM build AS publish
RUN dotnet publish "N5PermissionsAPI.Application.csproj" -c Release -o /app/publish

# Establece la imagen final para ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5PermissionsAPI.Application.dll"]
