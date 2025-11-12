# Dockerfile

# Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY ["Rrhh-backend/Rrhh-backend.csproj", "Rrhh-backend/"]
RUN dotnet restore "Rrhh-backend/Rrhh-backend.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar y publicar
WORKDIR "/src/Rrhh-backend"
RUN dotnet build "Rrhh-backend.csproj" -c Release -o /app/build
RUN dotnet publish "Rrhh-backend.csproj" -c Release -o /app/publish

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copiar archivos publicados
COPY --from=build /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "Rrhh-backend.dll"]