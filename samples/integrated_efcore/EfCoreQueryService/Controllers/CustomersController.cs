using System;
using System.Threading.Tasks;
using EfCoreQueryService.Projectors;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreQueryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        

        public CustomersController()
        {
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetCustomer([FromQuery]Guid id)
        {
            return Ok(CustomerCreatedEventProjector._CustomerCreated);
        }
    }
}