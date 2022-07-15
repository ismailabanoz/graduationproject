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
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
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
