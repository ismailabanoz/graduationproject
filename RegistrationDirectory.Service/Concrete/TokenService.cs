using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.DTOs;
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
        private readonly CustomTokenOption _tokenOption;
         private readonly AppDbContext _appDbContext;
         private readonly UserManager<AppUser> _userManager;

         public TokenService(IOptions<CustomTokenOption> tokenOption, AppDbContext appDbContext, UserManager<AppUser> userManager)
         {
             _tokenOption = tokenOption.Value;
             _appDbContext = appDbContext;
             _userManager = userManager;
         }

         public TokenDto CreateToken(string username)
         {
            /*var user = _userManager.FindByNameAsync(appUser.UserName);
            if (user == null)
            {
                return BadRequest("user not found");
            }*/



            var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);

          var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
          var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
          var securityKey =Encoding.ASCII.GetBytes(_tokenOption.SecurityKey);

          SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);

          JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
              issuer: _tokenOption.Issuer,
              expires: accessTokenExpiration,
               notBefore: DateTime.Now,
               claims: GetClaims(user, _tokenOption.Audience),
               signingCredentials: signingCredentials);

          var handler = new JwtSecurityTokenHandler();

          var token = handler.WriteToken(jwtSecurityToken);
          RefreshToken(user);
          var tokenDto = new TokenDto
          {
              AccessToken = token,
              AccessTokenExpiration = accessTokenExpiration,
              RefreshTokenExpiration = refreshTokenExpiration
          };

          return tokenDto;
      }

      private IEnumerable<Claim> GetClaims(AppUser appUser, List<String> audiences)
      {
          var userList = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier,appUser.Id),
          new Claim(ClaimTypes.Name,appUser.UserName),
          new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
          };

          userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

          return userList;
      }
      private void RefreshToken(AppUser appUser)
      {
          var refreshToken = CreateRefreshToken();
          var checkRefreshToken = _appDbContext.RefreshTokens.Find(appUser.UserName);
          if (checkRefreshToken == null)
          {
              _appDbContext.RefreshTokens.Add(new RefreshToken { UserName = appUser.UserName, Guid = refreshToken, ExpDate = DateTime.Now.AddDays(60) });
          }
          else
          {
              checkRefreshToken.Guid = refreshToken;
              checkRefreshToken.ExpDate = DateTime.Now.AddDays(60);
          }
          _appDbContext.SaveChanges();
      }
      private string CreateRefreshToken()
      {
          return Guid.NewGuid().ToString();
      }
        
        public TokenDto CreateToken(AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}
