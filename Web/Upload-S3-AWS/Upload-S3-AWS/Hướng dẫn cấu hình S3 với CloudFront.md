# Hướng dẫn cấu hình S3 với CloudFront

## Mục lục
- [Giới thiệu](#giới-thiệu)
- [Phần 1: Cài đặt S3 Bucket](#phần-1-cài-đặt-s3-bucket)
- [Phần 2: Cài đặt CloudFront Distribution](#phần-2-cài-đặt-cloudfront-distribution)
- [Phần 3: Cấu hình Origin Access Control (OAC)](#phần-3-cấu-hình-origin-access-control-oac)
- [Phần 4: Cập nhật Bucket Policy](#phần-4-cập-nhật-bucket-policy)
- [Phần 5: Cập nhật ứng dụng .NET](#phần-5-cập-nhật-ứng-dụng-net)
- [Khắc phục sự cố](#khắc-phục-sự-cố)

## Giới thiệu

Tài liệu này hướng dẫn cấu hình S3 Bucket kết hợp với CloudFront CDN để:
- Upload ảnh lên S3
- Phân phối ảnh qua CloudFront CDN
- Bảo mật nội dung S3 chỉ cho phép truy cập qua CloudFront

## Phần 1: Cài đặt S3 Bucket

### 1.1. Tạo S3 Bucket

1. Đăng nhập vào AWS Management Console
2. Vào dịch vụ S3
3. Chọn "Create bucket"
4. Điền thông tin:
   - Bucket name: `joytime-bucket`
   - Region: `ap-southeast-7` (Thailand)
   - Để các cài đặt khác là mặc định

### 1.2. Cấu hình S3 Bucket

1. Đảm bảo "Block all public access" được bật (khuyến nghị)
2. Kích hoạt versioning nếu cần
3. Không cần thiết lập CORS ở giai đoạn này

## Phần 2: Cài đặt CloudFront Distribution

### 2.1. Tạo CloudFront Distribution

1. Vào dịch vụ CloudFront
2. Chọn "Create Distribution"
3. Cài đặt Origin:
   - Origin domain: `joytime-bucket.s3.ap-southeast-7.amazonaws.com`
   - Origin path: Để trống
   - Origin ID: Để mặc định
   - Origin access: Để trống (sẽ cấu hình OAC sau)

4. Cài đặt Default cache behavior:
   - Path pattern: `*` (Default)
   - Compress objects automatically: Yes
   - Viewer protocol policy: Redirect HTTP to HTTPS
   - Allowed HTTP methods: GET, HEAD
   - Cache key and origin requests:
     - Cache policy: CachingOptimized
     - Origin request policy: CORS-S3Origin

5. Cài đặt Distribution:
   - Price class: Chọn theo ngân sách của bạn
   - WAF: Không cần thiết
   - Alternate domain names (CNAMEs): Để trống hoặc thêm domain của bạn
   - Custom SSL certificate: Không cần thiết cho giai đoạn đầu
   - Default root object: Để trống
   - Description: `CloudFront for joytime-bucket`

6. Nhấn "Create distribution"

### 2.2. Lấy domain CloudFront

Sau khi tạo distribution, bạn sẽ có domain CloudFront dạng: `dxxxxxxxxxxxxx.cloudfront.net`.
Lưu lại domain này để cấu hình trong ứng dụng.

## Phần 3: Cấu hình Origin Access Control (OAC)

### 3.1. Tạo OAC

1. Trong CloudFront console, chọn "Origin access"
2. Chọn "Create control setting"
3. Điền thông tin:
   - Name: `joytime-bucket-oac`
   - Description: `Cấu hình Origin Access Control cho S3 bucket joytime-bucket, cho phép CloudFront truy cập nội dung trong bucket trong khi ngăn truy cập trực tiếp từ người dùng.`
   - Signing behavior: Chọn **"Sign requests (recommended)"**

4. Chọn "Create"

### 3.2. Áp dụng OAC cho Origin

1. Trong CloudFront console, chọn distribution của bạn
2. Vào tab "Origins"
3. Chọn origin S3 và nhấn "Edit"
4. Trong phần "Origin access", chọn "Origin access control settings (recommended)"
5. Chọn OAC vừa tạo từ dropdown
6. Nhấn "Save changes"
7. AWS sẽ hiển thị một thông báo về việc cần cập nhật bucket policy, nhấn "Copy policy"

## Phần 4: Cập nhật Bucket Policy

1. Vào dịch vụ S3
2. Chọn bucket `joytime-bucket`
3. Vào tab "Permissions"
4. Trong phần "Bucket policy", chọn "Edit"
5. Dán policy mà AWS đã cung cấp, có dạng như sau:

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "AllowCloudFrontServicePrincipal",
            "Effect": "Allow",
            "Principal": {
                "Service": "cloudfront.amazonaws.com"
            },
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::joytime-bucket/*",
            "Condition": {
                "StringEquals": {
                    "AWS:SourceArn": "arn:aws:cloudfront::ACCOUNT_ID:distribution/DISTRIBUTION_ID"
                }
            }
        }
    ]
}
```

6. Nhấn "Save changes"

## Phần 5: Cập nhật ứng dụng .NET

### 5.1. Cập nhật appsettings.json

```json
{
  "AWS": {
    "Profile": "default",
    "Region": "ap-southeast-7",
    "S3": {
      "BucketName": "joytime-bucket"
    },
    "CloudFront": {
      "Domain": "dxxxxxxxxxxxxx.cloudfront.net"
    }
  }
}
```

### 5.2. Cập nhật S3Service để sử dụng CloudFront

```csharp
public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _cdnDomain;

    public S3Service(IAmazonS3 s3Client, string bucketName, string cdnDomain)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _cdnDomain = cdnDomain;
    }

    public async Task<UploadImageResponse> UploadImageAsync(IFormFile file)
    {
        try
        {
            // ... Code kiểm tra file ...

            // Tạo tên file duy nhất bằng cách thêm timestamp
            var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
            
            // Thiết lập key cho object S3
            var key = $"images/{fileName}";

            using (var stream = file.OpenReadStream())
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    // Thêm cache control để tối ưu CDN
                    CacheControl = "max-age=31536000"
                };

                await _s3Client.PutObjectAsync(request);
            }

            // Tạo CloudFront URL cho ảnh
            string imageUrl = $"https://{_cdnDomain}/{key}";

            return new UploadImageResponse
            {
                Success = true,
                ImageUrl = imageUrl,
                Message = "Upload thành công"
            };
        }
        catch (Exception ex)
        {
            return new UploadImageResponse
            {
                Success = false,
                Message = $"Lỗi khi upload: {ex.Message}"
            };
        }
    }
}
```

### 5.3. Cập nhật Program.cs để đọc cấu hình CloudFront

```csharp
var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình từ appsettings.json
var bucketName = builder.Configuration["AWS:S3:BucketName"];
var cdnDomain = builder.Configuration["AWS:CloudFront:Domain"];

// Kiểm tra cấu hình
if (string.IsNullOrEmpty(bucketName))
{
    throw new ArgumentException("AWS:S3:BucketName không được cấu hình");
}

if (string.IsNullOrEmpty(cdnDomain))
{
    throw new ArgumentException("AWS:CloudFront:Domain không được cấu hình");
}

// Đăng ký dịch vụ
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<IS3Service>(provider =>
{
    var s3Client = provider.GetRequiredService<IAmazonS3>();
    return new S3Service(s3Client, bucketName, cdnDomain);
});

// ... Code còn lại ...
```

## Khắc phục sự cố

### Lỗi "Access Denied" khi truy cập CloudFront URL

1. **Kiểm tra OAC đã được áp dụng đúng cách**:
   - Vào CloudFront > Origin > Kiểm tra Origin access

2. **Xác nhận Bucket Policy đã được cập nhật**:
   - Kiểm tra S3 > Permissions > Bucket policy

3. **Làm mới CloudFront Cache**:
   - Tạo invalidation với path `/*`

4. **Kiểm tra file tồn tại trong S3**:
   - Vào S3 console và kiểm tra file trong path `images/`

5. **Đợi CloudFront propagation**:
   - CloudFront có thể mất 5-15 phút để cập nhật thay đổi

6. **Kiểm tra AWS credentials**:
   - Đảm bảo IAM user có quyền S3:PutObject

### Lỗi "AWS Access Key Id does not exist"

Cập nhật AWS credentials bằng một trong các cách sau:

1. **Cấu hình AWS CLI**:
   ```
   aws configure --profile joytime-profile
   ```

2. **Biến môi trường**:
   ```
   set AWS_ACCESS_KEY_ID=your_access_key
   set AWS_SECRET_ACCESS_KEY=your_secret_key
   set AWS_REGION=ap-southeast-7
   ```

3. **Cấu hình trong code**:
   ```csharp
   var credentials = new BasicAWSCredentials("ACCESS_KEY", "SECRET_KEY");
   var s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName("ap-southeast-7"));
   ```