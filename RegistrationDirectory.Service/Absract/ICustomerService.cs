using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Absract
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        void Delete(int customerId);
        void Update(Customer customer);
        void Create(Customer customer);
        Customer GetById(int customerId);
    }
}
