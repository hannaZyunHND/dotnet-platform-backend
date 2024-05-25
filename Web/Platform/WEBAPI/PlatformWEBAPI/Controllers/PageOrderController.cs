using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PageOrderController : ControllerBase
    {
        private IOrderRepository _orderRepository;
        public PageOrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("CreateMultipleItemOrderAsync")]
        public async Task<IActionResult> CreateMultipleItemOrderAsync(RequestCreateMultipleItemOrder reqest)
        {
            var response = await _orderRepository.CreateMultipleItemOrderAsync(reqest);
            if(response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("ERROR");
            }
            
        }
    }
}
