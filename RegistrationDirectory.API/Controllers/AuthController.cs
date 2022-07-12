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
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly Watermark _watermark;
        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ITokenService tokenService, Watermark watermark)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _watermark = watermark;
        }
        [HttpGet]
        public async Task<IActionResult> GetToken([FromBody] AuthenticateRequestModel authenticateRequestModel)
        {
            var token = await _tokenService.CreateToken(authenticateRequestModel.Username, authenticateRequestModel.Password);
            return Ok(new
            {
                AccessToken = token
            });

        }
    }
}
