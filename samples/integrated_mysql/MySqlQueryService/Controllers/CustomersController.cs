using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlQueryService.MySqlConfig;
using MySqlQueryService.Projectors;

namespace MySqlQueryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        
        private MySqlDbContext _context;
        public CustomersController(MySqlDbContext context)
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