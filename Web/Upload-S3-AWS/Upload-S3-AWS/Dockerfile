# Sử dụng builder pattern với đa giai đoạn
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["Upload-S3-AWS.csproj", "./"]
# Sử dụng --no-cache để giảm kích thước layer
RUN dotnet restore "./Upload-S3-AWS.csproj" --no-cache

# Chỉ copy những file cần thiết
COPY . .
RUN dotnet publish "./Upload-S3-AWS.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Sử dụng image alpine cho môi trường runtime - nhẹ hơn nhiều so với image gốc
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
# Thiết lập biến môi trường để tối ưu hóa hiệu suất của .NET
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0

# Thiết lập timezone để tránh vấn đề về thời gian nếu cần
ENV TZ=Asia/Ho_Chi_Minh

# Chỉ copy những file cần thiết từ build stage
COPY --from=build /app/publish .

# Tạo thư mục .aws nếu bạn cần sử dụng AWS profile
RUN mkdir -p /root/.aws

# Expose các port nếu cần
EXPOSE 8080
EXPOSE 8081

# Sử dụng lệnh CMD thay vì ENTRYPOINT để dễ dàng ghi đè khi triển khai
CMD ["dotnet", "Upload-S3-AWS.dll"]