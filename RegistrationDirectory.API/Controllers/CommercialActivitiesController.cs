using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;

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
        public IActionResult GetAll()
        {
            var commercialActivities = _commercialActivityService.GetAll();
            return Ok(commercialActivities);
        }
        [HttpPost]
        public IActionResult Add(CommercialActivity commercialActivity)
        {
            _commercialActivityService.Create(commercialActivity);
            return Ok(commercialActivity);
        }
    }
}
