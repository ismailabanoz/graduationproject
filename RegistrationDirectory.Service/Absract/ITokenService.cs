using RegistrationDirectory.DataAccess.DTOs;
using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Absract
{
    public interface ITokenService
    {
        Task<string> CreateToken(string userName, string password);
    }
}
