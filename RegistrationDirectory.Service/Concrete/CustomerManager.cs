using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerManager(AppDbContext customer, IUnitOfWork unitOfWork)
        {
            _context = customer;
            _unitOfWork = unitOfWork;
        }

        public void Create(Customer customer)
        {
            _context.Add(customer);
            _unitOfWork.Commit();
        }

        public void Delete(int customerId)
        {
            _context.Remove(GetById(customerId));
            _unitOfWork.Commit();
        }

        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetById(int customerId)
        {
            return _context.Set<Customer>().SingleOrDefault(p => p.Id == customerId);
        }

        public void Update(Customer customer)
        {
            _context.Update(customer);
            _unitOfWork.Commit();
        }
    }
}
