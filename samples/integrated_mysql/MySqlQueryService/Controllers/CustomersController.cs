using System;
using Microsoft.AspNetCore.Mvc;
using MySqlQueryService.Projectors;

namespace MySqlQueryService.Controllers
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