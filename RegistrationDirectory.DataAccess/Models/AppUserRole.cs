using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public class AppUserRole : IdentityUserRole
    {
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
