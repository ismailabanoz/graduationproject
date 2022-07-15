using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;

namespace RegistrationDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles= "Admin,Editor")]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }
        [HttpGet("{customerId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult GetCustomer(int customerId)
        {
            var customers = _customerService.GetById(customerId);
            return Ok(customers);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Add(Customer customer)
        {
            _customerService.Create(customer);
            return Ok(customer);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Update(Customer customer)
        {
            _customerService.Update(customer);
            return Ok(customer);
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int customerId)
        {
            _customerService.Delete(customerId);
            return Ok();
        }
    }
}
