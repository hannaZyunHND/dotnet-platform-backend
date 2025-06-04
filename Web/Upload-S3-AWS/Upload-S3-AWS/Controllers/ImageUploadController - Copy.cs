//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Upload_S3_AWS.Models;
//using Upload_S3_AWS.Services;

//namespace Upload_S3_AWS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ImageController : ControllerBase
//    {
//        private readonly IS3Service _s3Service;
//        private readonly string _bucketName;

//        public ImageController(IS3Service s3Service)
//        {
//            _s3Service = s3Service;
//        }

//        [HttpPost("upload")]
//        public async Task<IActionResult> UploadImage(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//                return BadRequest("No file was uploaded");

//            var request = new UploadImageRequest
//            {
//                imageFile = file,
//                BucketName = _bucketName,
//                IsThumbnail = false
//            };

//            var result = await _s3Service.UploadImageAsync(request);

//            if (result.Success)
//                return Ok(result);
//            else
//                return BadRequest(result);
//        }

//        [HttpPost("upload-with-thumbnail")]
//        public async Task<IActionResult> UploadWithThumbnail(IFormFile file, [FromForm] int width = 300, [FromForm] int height = 300)
//        {
//            if (file == null || file.Length == 0)
//                return BadRequest("No file was uploaded");

//            var request = new UploadImageRequest
//            {
//                imageFile = file,
//                BucketName = _bucketName,
//                ThumbnailWidth = width,
//                ThumbnailHeight = height
//            };

//            var result = await _s3Service.UploadWithThumbnailAsync(request, width, height);

//            if (result.Success)
//                return Ok(result);
//            else
//                return BadRequest(result);
//        }
//    }
//}