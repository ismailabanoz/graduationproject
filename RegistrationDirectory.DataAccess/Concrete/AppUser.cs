using Microsoft.AspNetCore.Identity;
using RegistrationDirectory.DataAccess.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Concrete
{
    public class AppUser:IdentityUser,IEntity
    {
    }
}
