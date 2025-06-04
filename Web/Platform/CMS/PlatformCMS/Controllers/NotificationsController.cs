using MI.Dal.IDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpPost]
        [Route("PushNotificationCreateOrder")]
        public async Task<IActionResult> PushNotificationCreateOrder([FromBody] RequestPushNotificationCreateOrder request)
        {
            using (IDbContext context = new IDbContext())
            {
                //Kiem tra xem 
                return Ok();
            }

                
        }
    }



    public class RequestPushNotificationCreateOrder
    {
        public string email { get; set; }
        public string orderCode { get; set; }
    }
}
