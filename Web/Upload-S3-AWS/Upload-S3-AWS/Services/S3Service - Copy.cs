//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Amazon.S3;
//using Amazon.S3.Model;
//using Microsoft.AspNetCore.Http;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using Upload_S3_AWS.Models;

//namespace Upload_S3_AWS.Services
//{
//    public interface IS3Service
//    {
//        Task<UploadImageResponse> UploadImageAsync(UploadImageRequest req);
//        Task<UploadImageResponse> UploadWithThumbnailAsync(UploadImageRequest req);
//    }

//    public class UploadImageResponse
//    {
//        public string ImageUrl { get; set; }
//        public string ThumbnailUrl { get; set; }
//        public bool Success { get; set; }
//        public string Message { get; set; }
//    }

//    public class ThumbnailOptions
//    {
//        public int Width { get; set; } = 300;
//        public int Height { get; set; } = 300;
//        public bool MaintainAspectRatio { get; set; } = true;
//    }

//    public class S3Service : IS3Service
//    {
//        private readonly IAmazonS3 _s3Client;

//        public S3Service(IAmazonS3 s3Client)
//        {
//            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
//        }

//        public async Task<UploadImageResponse> UploadImageAsync(UploadImageRequest req)
//        {
//            var file = req.imageFile;
//            try
//            {
//                // Kiểm tra file có tồn tại không
//                if (file == null || file.Length == 0)
//                {
//                    return new UploadImageResponse
//                    {
//                        Success = false,
//                        Message = "Vui lòng chọn file để upload"
//                    };
//                }

//                // Kiểm tra định dạng file
//                var extension = Path.GetExtension(file.FileName).ToLower();
//                if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp"))
//                {
//                    return new UploadImageResponse
//                    {
//                        Success = false,
//                        Message = "Chỉ hỗ trợ các định dạng ảnh: jpg, jpeg, png, gif, webp"
//                    };
//                }

//                // Tạo tên file duy nhất bằng cách thêm timestamp
//                var fileName = Path.GetFileName(file.FileName);
//                var year_current = DateTime.Now.Year.ToString();
//                var month_current = DateTime.Now.Month.ToString("D2");
//                var day_current = DateTime.Now.Day.ToString("D2");

//                // Thiết lập key cho object S3 (đường dẫn trong bucket)
//                var urlImage = $"{year_current}/{month_current}/{day_current}/{fileName}";

//                var key = $"uploads/{urlImage}";

//                // Chuẩn bị request để upload lên S3
//                using (var stream = file.OpenReadStream())
//                {
//                    var request = new PutObjectRequest
//                    {
//                        BucketName = req.BucketName,
//                        Key = key,
//                        InputStream = stream,
//                        ContentType = file.ContentType
//                    };

//                    // Thêm các header caching cho CDN nếu cần
//                    request.Headers.CacheControl = "max-age=31536000"; // Cache 1 năm

//                    // Thực hiện upload
//                    await _s3Client.PutObjectAsync(request);
//                }

//                return new UploadImageResponse
//                {
//                    Success = true,
//                    ImageUrl = $"/{urlImage}",
//                    Message = "Upload thành công"
//                };
//            }
//            catch (Exception ex)
//            {
//                return new UploadImageResponse
//                {
//                    Success = false,
//                    Message = $"Lỗi khi upload: {ex.Message}"
//                };
//            }
//        }

//        /// <summary>
//        /// Tạo và upload cả ảnh gốc và thumbnail
//        /// </summary>
//        public async Task<UploadImageResponse> UploadWithThumbnailAsync(UploadImageRequest req)
//        {
//            var file = req.imageFile;
//            try
//            {
//                // Kiểm tra file có tồn tại không
//                if (file == null || file.Length == 0)
//                {
//                    return new UploadImageResponse
//                    {
//                        Success = false,
//                        Message = "Vui lòng chọn file để upload"
//                    };
//                }

//                // Kiểm tra định dạng file
//                var extension = Path.GetExtension(file.FileName).ToLower();
//                if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp"))
//                {
//                    return new UploadImageResponse
//                    {
//                        Success = false,
//                        Message = "Chỉ hỗ trợ các định dạng ảnh: jpg, jpeg, png, gif, webp"
//                    };
//                }

//                // Tạo tên file
//                var fileName = Path.GetFileName(file.FileName);
//                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
//                var year_current = DateTime.Now.Year.ToString();
//                var month_current = DateTime.Now.Month.ToString("D2");
//                var day_current = DateTime.Now.Day.ToString("D2");

//                // Đường dẫn chung
//                var baseUrlPath = $"{year_current}/{month_current}/{day_current}";

//                // Đường dẫn ảnh gốc và thumbnail - sử dụng cùng tên file
//                var imagePath = $"{baseUrlPath}/{fileName}";

//                // Key cho S3 - chỉ khác nhau ở thư mục
//                var originalKey = $"uploads/{imagePath}";
//                var thumbnailKey = $"uploads/thumb/{imagePath}";

//                // 1. Upload ảnh gốc
//                using (var stream = file.OpenReadStream())
//                {
//                    var request = new PutObjectRequest
//                    {
//                        BucketName = req.BucketName,
//                        Key = originalKey,
//                        InputStream = stream,
//                        ContentType = file.ContentType
//                    };
//                    request.Headers.CacheControl = "max-age=31536000";
//                    await _s3Client.PutObjectAsync(request);
//                }

//                // 2. Tạo và upload thumbnail
//                using (var memoryStream = new MemoryStream())
//                {
//                    // Đọc file gốc
//                    await file.CopyToAsync(memoryStream);
//                    memoryStream.Position = 0;

//                    // Tạo thumbnail với ImageSharp
//                    using (var thumbnailStream = new MemoryStream())
//                    {
//                        using (var image = await Image.LoadAsync(memoryStream))
//                        {
//                            // Tính toán kích thước để giữ tỉ lệ
//                            var options = new ThumbnailOptions
//                            {
//                                Width = thumbWidth,
//                                Height = thumbHeight
//                            };

//                            if (options.MaintainAspectRatio)
//                            {
//                                var originalWidth = image.Width;
//                                var originalHeight = image.Height;
//                                var ratio = (float)originalWidth / originalHeight;

//                                if (originalWidth > originalHeight)
//                                {
//                                    options.Height = (int)(options.Width / ratio);
//                                }
//                                else
//                                {
//                                    options.Width = (int)(options.Height * ratio);
//                                }
//                            }

//                            // Resize ảnh
//                            image.Mutate(x => x.Resize(options.Width, options.Height));

//                            // Lưu vào stream
//                            await image.SaveAsync(thumbnailStream, GetImageFormat(extension));
//                            thumbnailStream.Position = 0;

//                            // Upload thumbnail lên S3
//                            var request = new PutObjectRequest
//                            {
//                                BucketName = req.BucketName,
//                                Key = thumbnailKey,
//                                InputStream = thumbnailStream,
//                                ContentType = GetContentType(extension)
//                            };
//                            request.Headers.CacheControl = "max-age=31536000";
//                            await _s3Client.PutObjectAsync(request);
//                        }
//                    }
//                }

//                return new UploadImageResponse
//                {
//                    Success = true,
//                    ImageUrl = $"/{imagePath}",
//                    ThumbnailUrl = $"/thumb/{imagePath}",
//                    Message = "Upload ảnh và thumbnail thành công"
//                };
//            }
//            catch (Exception ex)
//            {
//                return new UploadImageResponse
//                {
//                    Success = false,
//                    Message = $"Lỗi khi xử lý và upload ảnh: {ex.Message}"
//                };
//            }
//        }

//        private SixLabors.ImageSharp.Formats.IImageFormat GetImageFormat(string extension)
//        {
//            return extension.ToLower() switch
//            {
//                ".jpg" or ".jpeg" => SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance,
//                ".png" => SixLabors.ImageSharp.Formats.Png.PngFormat.Instance,
//                ".gif" => SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance,
//                ".webp" => SixLabors.ImageSharp.Formats.Webp.WebpFormat.Instance,
//                _ => SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance
//            };
//        }

//        private string GetContentType(string extension)
//        {
//            return extension.ToLower() switch
//            {
//                ".jpg" or ".jpeg" => "image/jpeg",
//                ".png" => "image/png",
//                ".gif" => "image/gif",
//                ".webp" => "image/webp",
//                _ => "image/jpeg"
//            };
//        }
//    }
//}