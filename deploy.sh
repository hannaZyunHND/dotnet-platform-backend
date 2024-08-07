#!/usr/bin/env bash

# Xóa cache build Docker
docker builder prune -f

# Xóa các image không cần thiết
docker image prune -a -f

# Xóa các container đã dừng
docker container prune -f

# Xóa các volume không cần thiết
docker volume prune -f

# Xóa tất cả các đối tượng không còn được sử dụng
docker system prune -a -f

# Dừng các container đang chạy
docker-compose -f Web/Platform/CMS/PlatformCMS/docker-compose.yml down

# Xóa các container cũ và các network liên quan
docker-compose -f Web/Platform/CMS/PlatformCMS/docker-compose.yml rm -f

# Pull image mới từ Docker Hub
docker-compose -f Web/Platform/CMS/PlatformCMS/docker-compose.yml pull

# Chạy container mới
docker-compose -f Web/Platform/CMS/PlatformCMS/docker-compose.yml up -d