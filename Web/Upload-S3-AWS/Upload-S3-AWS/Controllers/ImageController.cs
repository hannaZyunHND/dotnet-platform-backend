using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Upload_S3_AWS.Models;
using Upload_S3_AWS.Services;

namespace Upload_S3_AWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IS3Service _s3Service;

        public ImageController(IS3Service s3Service, IConfiguration configuration)
        {
            _s3Service = s3Service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded");

            var request = new UploadImageRequest
            {
                imageFile = file,
            };

            var result = await _s3Service.UploadImageAsync(request);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("upload-with-webp")]
        public async Task<IActionResult> UploadWithWebpConversion(UploadImageRequest req)
        {
            if (req.imageFile == null || req.imageFile.Length == 0)
                return BadRequest("No file was uploaded");

            var result = await _s3Service.UploadWithWebpConversionAsync(req);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}