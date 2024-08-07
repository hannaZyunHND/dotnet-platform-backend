using MI.Entity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Article.Repository;
using PlatformWEBAPI.Services.Article.ViewModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageArticlesController : ControllerBase
    {

        private readonly IArticleRepository _articleRepository;
        public PageArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpPost]
        [Route("GetBlogDetailById")]
        public async Task<IActionResult> GetBlogDetailById(RequestGetBlogDetailById request)
        {
            if (request != null)
            {
                var response = _articleRepository.GetArticleDetail(request.id, request.cultureCode);
                if (response != null)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("GetBlogsSameZone")]
        public async Task<IActionResult> GetBlogsSameZone(RequestGetBlogsSameZone request)
        {
            if(request != null)
            {
                var _t = 0;

                var response = _articleRepository.GetArticlesInZoneId_Minify_FullFilter(request.zoneId, (int)TypeZone.All, request.type, 0, request.cultureCode, "", 1, 6, out _t);
                response = response.Where(r => r.Id != request.blogId).ToList();
                return Ok(response);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("GetBlogsInZone")]
        public async Task<IActionResult> GetBlogsInZone(RequestGetBlogsInZone request)
        {
            if (request != null)
            {
                var _t = 0;

                var result = _articleRepository.GetArticlesInZoneId_Minify_FullFilter(request.zoneId, (int)TypeZone.All, (int)TypeArticle.All, 2, request.cultureCode, "", request.pageIndex, request.pageSize, out _t);
                //response = response.Where(r => r.Id != request.blogId).ToList();

                dynamic response = new ExpandoObject();
                var totalPage = _t / request.pageSize;
                if (_t % request.pageSize > 0)
                {
                    totalPage++;
                }
                response.totalPage = totalPage;
                response.firstItem = result.FirstOrDefault();
                response.nextThreeItem = result.Skip(1).Take(3).ToList();
                response.lastItems = result.Skip(4).ToList();
                return Ok(response);
            }
            return BadRequest();
        }

    }
}
