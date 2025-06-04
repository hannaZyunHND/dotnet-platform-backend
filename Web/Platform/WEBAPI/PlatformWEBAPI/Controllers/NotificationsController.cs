using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MI.Dal.IDbContext;
using Microsoft.EntityFrameworkCore;
using PlatformWEBAPI.Services.Extra.ViewModel;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpPost]
        [Route("SaveFcmToken")]
        public async Task<IActionResult> SaveFcmToken([FromBody] SaveFcmTokenRequest req)
        {
            using (IDbContext context = new IDbContext())
            {
                var existing = await context.CustomerFcmToken.FirstOrDefaultAsync(x => x.Email == req.Email);

                if (existing != null)
                {
                    existing.FcmToken = req.FcmToken;
                    existing.CreatedAt = DateTime.Now;
                }
                else
                {
                    context.CustomerFcmToken.Add(new CustomerFcmToken
                    {
                        Email = req.Email,
                        FcmToken = req.FcmToken
                    });
                }

                await context.SaveChangesAsync();
                return Ok();
            }
                
        }

    }
}
