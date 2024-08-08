# Stage 1: Build .NET Core application
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine AS build
WORKDIR /src

# Copy and restore .NET Core project dependencies
COPY *.sln .
COPY Web/Platform/WEBAPI/PlatformWEBAPI/*.csproj ./Web/Platform/WEBAPI/PlatformWEBAPI/
COPY Common/Utils/*.csproj ./Common/Utils/
COPY Core/IBo/*.csproj ./Core/IBo/
COPY Core/IDal/*.csproj ./Core/IDal/
COPY Core/IEntity/*.csproj ./Core/IEntity/
COPY Core/MI.Cache/*.csproj ./Core/MI.Cache/
COPY MI.Dapper.Data/*.csproj ./MI.Dapper.Data/
COPY MI.ES/*.csproj ./MI.ES/
COPY MI.Service/*.csproj ./MI.Service/
COPY Mi.MemoryCache/*.csproj ./Mi.MemoryCache/

RUN dotnet restore Web/Platform/WEBAPI/PlatformWEBAPI/PlatformWEBAPI.csproj

# Add ARG variable to allow specifying the build configuration (Release or Debug)
ARG ENVIRONMENT=Production

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