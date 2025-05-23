using MI.Dapper.Data.Constant;
using MI.Dapper.Data.Models;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Dapper.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPermissionRepository _permissionRepository;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IConfiguration configuration, IPermissionRepository permissionRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _permissionRepository = permissionRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
                if (!result.Succeeded)
                    return BadRequest("Mật khẩu không đúng");

                var roles = string.Join(";", await _userManager.GetRolesAsync(user));
                var claims = new[]
                {
                    new Claim("Email", user.Email),
                    new Claim(SystemConstants.UserClaim.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(SystemConstants.UserClaim.Roles, string.Join(";", roles)),
                    new Claim(SystemConstants.UserClaim.FullName, user.FullName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                    _configuration["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: creds);
                var tokenJwt = new JwtSecurityTokenHandler().WriteToken(token);
                var currentUser = new CurrentUserViewModel()
                {
                    Email = user.Email,
                    Id = user.Id.ToString(),
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Token = tokenJwt
                };
                return Ok(currentUser);
            }

            return NotFound($"Không tìm thấy tài khoản {model.UserName}");
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new AppUser
            { FullName = model.FullName, UserName = model.Email, Email = model.Email, InActive = true };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(model);
            }
            return BadRequest();
        }
    }
}
