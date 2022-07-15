using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using RegistrationDirectory.Service.Concrete;

namespace RegistrationDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommercialActivitiesController : ControllerBase
    {
        private readonly ICommercialActivityService _commercialActivityService;

        public CommercialActivitiesController(ICommercialActivityService commercialActivityService)
        {
            _commercialActivityService = commercialActivityService;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult GetAll()
        {
            var commercialActivities = _commercialActivityService.GetAll();
            return Ok(commercialActivities);
        }
        [HttpGet("{commercialActivityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult GetCustomer(int commercialActivityId)
        {
            var commercialActivity = _commercialActivityService.GetById(commercialActivityId);
            return Ok(commercialActivity);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Add(CommercialActivity commercialActivity)
        {
            _commercialActivityService.Create(commercialActivity);
            return Ok(commercialActivity);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Update(CommercialActivity commercialActivity)
        {
            _commercialActivityService.Update(commercialActivity);
            return Ok(commercialActivity);
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int commercialActivityId)
        {
            _commercialActivityService.Delete(commercialActivityId);
            return Ok();
        }
    }
}
