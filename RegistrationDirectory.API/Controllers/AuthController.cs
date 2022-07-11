using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using RegistrationDirectory.Service.Concrete;

namespace RegistrationDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(ITokenService tokenService, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken(string username,string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            { 
                return BadRequest("invalid username or password"); 
            }
            if (!await _userManager.CheckPasswordAsync(user,password))
            {
                return BadRequest("invalid username or password");
            }

            return Ok(_tokenService.CreateToken(username));
        }
        [HttpGet("createuser")]
        public async Task< IActionResult> CreateUser()
        {
            await _roleManager.CreateAsync(new() { Name = "admin" });
            await _roleManager.CreateAsync(new() { Name = "manager" });
            var user = new AppUser() { UserName = "test" };

            await _userManager.CreateAsync(user, "Password12*");
            await _userManager.AddToRoleAsync(user, "admin");
            return Ok();
        }
    }
}
