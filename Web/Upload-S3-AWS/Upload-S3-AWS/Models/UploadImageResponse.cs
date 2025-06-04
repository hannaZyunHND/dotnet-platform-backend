using Microsoft.AspNetCore.Http;

namespace Upload_S3_AWS.Models
{
    public class UploadImageRequest
    {
        public IFormFile imageFile { get; set; }
        public string BucketName { get; set; }

        // Cấu hình WebP
        public int WebpQuality { get; set; } = 80; // Mặc định chất lượng 80%
        public bool WebpLossless { get; set; } = false; // Mặc định sử dụng lossy (nén có mất dữ liệu)

        //public bool IsThumbnail { get; set; } = false;
        //public int ThumbnailWidth { get; set; } = 300;
        //public int ThumbnailHeight { get; set; } = 300;
    }
}