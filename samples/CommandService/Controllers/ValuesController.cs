using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommandService.Controllers
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
           return Ok(new  { Response  = "Command Service"} );
        }
    }
}
