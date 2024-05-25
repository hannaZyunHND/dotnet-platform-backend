using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IExtraRepository _extraRepository;
        public AuthController(IOrderRepository orderRepository, IExtraRepository extraRepository)
        {
            _orderRepository = orderRepository;
            _extraRepository = extraRepository;
        }
        [HttpPost]
        [Route("DoLogin")]
        public async Task<IActionResult> DoLogin(CustomerAuthViewModel request)
        {
            var result = await _orderRepository.DoLogin(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(CustomerAuthViewModel request)
        {
            var result = _orderRepository.ChangePassword(request);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        [Route("DoSignUp")]
        public async Task<IActionResult> DoSignUp(CustomerAuthViewModel request)
        {
            var result = _orderRepository.DoSignUp(request);
            if (result > 0)
            {
                _extraRepository.SendEmailRegister(request);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
