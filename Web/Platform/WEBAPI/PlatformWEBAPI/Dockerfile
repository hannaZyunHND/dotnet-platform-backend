# Stage 1: Build .NET Core application
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine AS build
WORKDIR /src

# Copy and restore .NET Core project dependencies
COPY *.sln .
COPY Web/Platform/WEBAPI/PlatformWEBAPI/*.csproj ./Web/Platform/WEBAPI/PlatformWEBAPI/
COPY Common/**/*.csproj ./Common/
COPY Core/**/*.csproj ./Core/
COPY MI.**/*.csproj ./MI/
COPY Mi.MemoryCache/*.csproj ./Mi.MemoryCache/

RUN dotnet restore Web/Platform/WEBAPI/PlatformWEBAPI/PlatformWEBAPI.csproj
ARG ENVIRONMENT=Development

# Copy everything else and build .NET Core application
COPY . .

# Copy the correct environment file based on the ENVIRONMENT variable
RUN cp Web/Platform/WEBAPI/PlatformWEBAPI/appsettings.${ENVIRONMENT}.json Web/Platform/WEBAPI/PlatformWEBAPI/appsettings.json

WORKDIR /src/Web/Platform/WEBAPI/PlatformWEBAPI
RUN dotnet publish -c Release -o /app/publish 

# Stage 3: Final production stage
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-alpine AS final
WORKDIR /src

# Copy .NET Core published output
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Run .NET Core application
CMD ["dotnet", "PlatformWEBAPI.dll"]