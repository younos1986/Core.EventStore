using System;
using Microsoft.AspNetCore.Mvc;
using PostgresQueryService.Projectors;

namespace PostgresQueryService.Controllers
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