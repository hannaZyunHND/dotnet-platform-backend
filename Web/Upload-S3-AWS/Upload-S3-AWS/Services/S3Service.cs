using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using Upload_S3_AWS.Models;

namespace Upload_S3_AWS.Services
{
    public interface IS3Service
    {
        Task<UploadImageResponse> UploadImageAsync(UploadImageRequest req);
        Task<UploadImageResponse> UploadWithWebpConversionAsync(UploadImageRequest req);
    }

    public class UploadImageResponse
    {
        public string ImageUrl { get; set; }
        public string WebpUrl { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class WebpOptions
    {
        public int Quality { get; set; } = 80;
        public bool Lossless { get; set; } = false;
        public int FileSize { get; set; } = 0;
    }

    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;

        public S3Service(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
        }

        public async Task<UploadImageResponse> UploadImageAsync(UploadImageRequest req)
        {
            var file = req.imageFile;
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new UploadImageResponse
                    {
                        Success = false,
                        Message = "Vui lòng chọn file để upload"
                    };
                }

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp"))
                {
                    return new UploadImageResponse
                    {
                        Success = false,
                        Message = "Chỉ hỗ trợ các định dạng ảnh: jpg, jpeg, png, gif, webp"
                    };
                }

                var fileName = Path.GetFileName(file.FileName);
                var year_current = DateTime.Now.Year.ToString();
                var month_current = DateTime.Now.Month.ToString("D2");
                var day_current = DateTime.Now.Day.ToString("D2");

                var urlImage = $"{year_current}/{month_current}/{day_current}/{fileName}";

                var key = $"uploads/{urlImage}";

                using (var stream = file.OpenReadStream())
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = req.BucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = file.ContentType
                    };

                    // Thêm các header caching cho CDN nếu cần
                    request.Headers.CacheControl = "max-age=31536000"; // Cache 1 năm

                    await _s3Client.PutObjectAsync(request);
                }

                return new UploadImageResponse
                {
                    Success = true,
                    ImageUrl = $"/{urlImage}",
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

        public async Task<UploadImageResponse> UploadWithWebpConversionAsync(UploadImageRequest req)
        {
            var file = req.imageFile;

            if (file == null || file.Length == 0)
            {
                return new UploadImageResponse
                {
                    Success = false,
                    Message = "Vui lòng chọn file để upload"
                };
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                return new UploadImageResponse
                {
                    Success = false,
                    Message = "Chỉ hỗ trợ các định dạng ảnh: jpg, jpeg, png, gif, webp"
                };
            }

            try
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = Path.GetFileName(file.FileName);

                var now = DateTime.Now;
                var baseUrlPath = $"{now.Year}/{now:MM}/{now:dd}";
                var imagePath = $"{baseUrlPath}/{fileName}";
                var webpFileName = $"{fileNameWithoutExtension}.webp";
                var webpPath = $"{baseUrlPath}/{webpFileName}";

                // Key cho S3
                var originalKey = $"uploads/{webpPath}";
                var webpKey = $"uploads/thumb/{webpPath}";

                // Khởi tạo các task và chạy song song
                using var memoryStream = new MemoryStream();

                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var originalData = memoryStream.ToArray();

                var uploadOriginalTask = ConvertAndUploadWebpAsync(
                    req.BucketName,
                    originalKey,
                    new MemoryStream(originalData),100);

                var uploadWebpTask = ConvertAndUploadWebpAsync(
                    req.BucketName,
                    webpKey,
                    new MemoryStream(originalData),
                    req.WebpQuality > 0 ? req.WebpQuality : 80);
                // var uploadWebpTask = ConvertAndUploadWebpAsync(
                //     req.BucketName,
                //     webpKey,
                //     new MemoryStream(originalData),
                //     req.WebpQuality > 0 ? req.WebpQuality : 80);

                
                // Chờ cả hai task hoàn thành song song
                await Task.WhenAll(uploadOriginalTask, uploadWebpTask);

                return new UploadImageResponse
                {
                    Success = true,
                    ImageUrl = $"/{webpPath}",
                    WebpUrl = $"/thumb/{webpPath}",
                    Message = "Upload ảnh gốc và chuyển đổi WebP thành công"
                };
            }
            catch (Exception ex)
            {
                return new UploadImageResponse
                {
                    Success = false,
                    Message = $"Lỗi khi xử lý và upload ảnh: {ex.Message}"
                };
            }
        }

        // Tách thành các method riêng biệt để dễ quản lý và tái sử dụng
        private async Task UploadOriginalImageAsync(string bucketName, string key, Stream imageStream, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = imageStream,
                ContentType = contentType,
                Headers = { CacheControl = "max-age=31536000" }
            };

            await _s3Client.PutObjectAsync(request);
        }

        private async Task ConvertAndUploadWebpAsync(string bucketName, string key, Stream imageStream, int quality)
        {
            using var webpStream = new MemoryStream();
            using var image = await Image.LoadAsync(imageStream);

            // Cấu hình chất lượng WebP
            var webpEncoder = new WebpEncoder
            {
                Quality = quality,
                FileFormat = WebpFileFormatType.Lossy
            };

            // Lưu vào stream dưới định dạng WebP
            await image.SaveAsWebpAsync(webpStream, webpEncoder);
            webpStream.Position = 0;

            // Upload phiên bản WebP lên S3
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = webpStream,
                ContentType = "image/webp",
                Headers = { CacheControl = "max-age=31536000" }
            };

            await _s3Client.PutObjectAsync(request);
        }


        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };
        }
    }
}