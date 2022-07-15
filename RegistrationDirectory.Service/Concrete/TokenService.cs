using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOption _customTokenOption;
         private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(AppDbContext appDbContext, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IOptions<CustomTokenOption> customTokenOption)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _customTokenOption = customTokenOption.Value;
        }

        public async Task<string> CreateToken(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "incorrect username or password";
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return "incorrect username or password";
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);
            var audience = (_customTokenOption.Audience[0]);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customTokenOption.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            userRoles.ToList().ForEach(x =>
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            });
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: claims,
                signingCredentials: signingCredentials);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            RefreshToken(user);
            return token;
        }
      
      private void RefreshToken(AppUser appUser)
      {
          var refreshToken = CreateRefreshToken();
          var checkRefreshToken = _appDbContext.RefreshTokens.Find(appUser.UserName);
          if (checkRefreshToken == null)
          {
              _appDbContext.RefreshTokens.Add(new RefreshTokenModel { UserName = appUser.UserName, Guid = refreshToken, ExpDate = DateTime.Now.AddDays(60) });
          }
          else
          {
              checkRefreshToken.Guid = refreshToken;
              checkRefreshToken.ExpDate = DateTime.UtcNow.AddDays(60);
          }
            _unitOfWork.Commit();
      }
      private string CreateRefreshToken()
      {
          return Guid.NewGuid().ToString();
      }
       
    }
}
