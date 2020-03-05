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

        //Core.EventStore.Dependencies.IEventStoreReader _eventStoreReader;
        public ValuesController()//Core.EventStore.Dependencies.IEventStoreReader eventStoreReader)
        {
          //  _eventStoreReader = eventStoreReader;

        }

        [HttpGet]
        public IActionResult  Get()
        {

            //var streams = _eventStoreReader.PerformAll().GetAwaiter();

           return Ok(new  { Response  = "Query Service"} );
        }
    }
}
