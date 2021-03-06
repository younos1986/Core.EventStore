using System;
using System.Threading.Tasks;
using IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoQueryService.MongoDbConfigs;

namespace MongoQueryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        readonly IMongoDb _mongoDb;

        public CustomersController(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetCustomer([FromQuery]Guid id)
        {
            var customer = await _mongoDb.GetOneAsync(id);
            return Ok(customer);
        }
    }
}