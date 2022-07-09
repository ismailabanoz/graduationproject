using RegistrationDirectory.DataAccess.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Concrete
{
    public class Customer : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public byte Photograph { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public List<CommercialActivity> CommercialActivity { get; set; }
    }
}
