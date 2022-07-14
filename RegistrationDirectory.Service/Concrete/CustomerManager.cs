using RegistirationDirectory.SharedLibrary;
using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private IUnitOfWork _unitOfWork;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerManager(IUnitOfWork unitOfWork, RabbitMQPublisher rabbitMQPublisher, IRepository<Customer> customerRepository)
        {
            _unitOfWork = unitOfWork;
            _rabbitMQPublisher = rabbitMQPublisher;
            _customerRepository = customerRepository;
        }

        public void Create(Customer customer)
        {
            _customerRepository.Add(customer);
            _unitOfWork.Commit();
            CreatPicture(customer);
        }

        private void CreatPicture(Customer customer)
        {
                
            
            _rabbitMQPublisher.PublishForCreatePicture(new CreatePictureMessage() { CustomerId=customer.Id,Name=customer.Name,SurName=customer.Surname, BytePhoto = customer.Photograph.ToArray() });
            _rabbitMQPublisher.PublishForWatermark(customer);
        }

        public void Delete(int customerId)
        {
            _customerRepository.Delete(customerId);
            _unitOfWork.Commit();
        }

        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetById(int customerId)
        {
            return _customerRepository.GetById(customerId);
        }

        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
            _unitOfWork.Commit();
        }
    }
}
