using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Absract
{
    public interface ICustomerDal:IRepository<Customer>
    {
    }
}
