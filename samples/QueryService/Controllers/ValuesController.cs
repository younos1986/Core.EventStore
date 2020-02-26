using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {


        public ValuesController()
        {
        }

        [HttpGet]
        public IActionResult  Get()
        {
           return Ok(new  { Response  = "Query Service"} );
        }
    }
}
