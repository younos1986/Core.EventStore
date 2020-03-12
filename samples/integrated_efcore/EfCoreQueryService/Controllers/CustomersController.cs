using System;
using System.Threading.Tasks;
using EfCoreQueryService.EfCoreConfig;
using EfCoreQueryService.Projectors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQueryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        

        private EfCoreDbContext _context;
        public CustomersController(EfCoreDbContext context)
        {
            _context = context;
        }


        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetCustomer([FromQuery]Guid id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(q => q.Id == id);
            if (customer == null)
                return NoContent();
            
            return Ok(customer);
        }
    }
}