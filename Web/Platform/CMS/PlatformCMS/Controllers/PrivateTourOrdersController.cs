using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateTourOrdersController : ControllerBase
    {
        [HttpPost]
        [Route("CreatePrivateTourOrder")]
        public async Task<IActionResult> CreatePrivateTourOrder(PrivateTourOrder request)
        {
            try
            {
                using (IDbContext context = new IDbContext())
                {
                    context.PrivateTourOrders.Add(request);
                    context.SaveChanges();
                    return Ok(request);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest("Có lỗi xảy ra, vui lòng liên hệ QTV");
            
        }
    }

}
