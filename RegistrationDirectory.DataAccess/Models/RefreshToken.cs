using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public class RefreshToken
    {
        [Key]
        public string UserName { get; set; }
        public string Guid { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
