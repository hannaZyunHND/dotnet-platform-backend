# Stage 1: Build .NET Core application
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine AS build
WORKDIR /src

# Copy and restore .NET Core project dependencies
COPY *.sln .
# Copy toàn bộ các tệp .csproj từ các thư mục con khác nhau
COPY Web/Platform/CMS/PlatformCMS/*.csproj ./Web/Platform/CMS/PlatformCMS/
COPY Common/**/*.csproj ./Common/
COPY Core/**/*.csproj ./Core/
COPY MI.**/*.csproj ./MI/
COPY Mi.MemoryCache/*.csproj ./Mi.MemoryCache/

RUN dotnet restore Web/Platform/CMS/PlatformCMS/PlatformCMS.csproj

# Copy everything else and build .NET Core application
COPY . .

# Copy the correct environment file based on the ENVIRONMENT variable
RUN cp Web/Platform/CMS/PlatformCMS/appsettings.Development.json Web/Platform/CMS/PlatformCMS/appsettings.json

WORKDIR /src//Web/Platform/CMS/PlatformCMS
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Build Vue.js application
FROM node:14.21.3-buster AS build-vue
WORKDIR /app

# Set environment variables for Node.js build and Python
ENV NODE_GYP_FORCE_PYTHON=/usr/bin/python3 \
    NPM_CONFIG_PYTHON=/usr/bin/python3 \
    PYTHON=/usr/bin/python3

# Install dependencies
RUN apt-get update && apt-get install -y python3 python3-pip make g++ build-essential libpng-dev \
    && ln -sf /usr/bin/python3 /usr/bin/python \
    && rm -rf /var/lib/apt/lists/*

# Copy and install Node.js dependencies
COPY Web/Platform/CMS/PlatformCMS/package*.json ./

# Remove package-lock.json if it exists
RUN npm ci

# Copy the rest of the Vue.js project files
COPY /Web/Platform/CMS/PlatformCMS/. .

# Fix Vue.js build errors (copy directly)
COPY Web/Platform/CMS/PlatformCMS/ClientApp/assets ./ClientApp/assets
COPY Web/Platform/CMS/PlatformCMS/ClientApp/router ./ClientApp/router
COPY Web/Platform/CMS/PlatformCMS/ClientApp/pages ./ClientApp/pages
COPY Web/Platform/CMS/PlatformCMS/ClientApp/pages/article/list.vue ./ClientApp/pages/Article/list.vue
COPY Web/Platform/CMS/PlatformCMS/ClientApp/pages/article/edit.vue ./ClientApp/pages/Article/edit.vue
COPY Web/Platform/CMS/PlatformCMS/ClientApp/pages/color/list.vue ./ClientApp/pages/Color/list.vue
COPY Web/Platform/CMS/PlatformCMS/ClientApp/pages/customerLucky/list.vue ./ClientApp/pages/CustomerLucky/list.vue
COPY Web/Platform/CMS/PlatformCMS/ClientApp/App.vue ./ClientApp/App.vue
COPY Web/Platform/CMS/PlatformCMS/ClientApp/main.js ./ClientApp/main.js
COPY Web/Platform/CMS/PlatformCMS/ClientApp/store ./ClientApp/store
COPY Web/Platform/CMS/PlatformCMS/webpack.config.js ./

# Build Vue.js project
RUN npm run build

# Stage 3: Final production stage
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-alpine AS final
WORKDIR /src

# Copy Vue.js build output
COPY --from=build-vue /app/wwwroot /src/wwwroot

# Copy .NET Core published output
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Run .NET Core application
CMD ["dotnet", "PlatformCMS.dll"]