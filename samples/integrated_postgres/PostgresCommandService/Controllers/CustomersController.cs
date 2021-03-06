﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostgresCommandService.Commands;
using PostgresCommandService.Dtos;

namespace PostgresCommandService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            CustomerDto customer = await _mediator.Send(createCustomerCommand);
            return Ok(customer);
        }

        //[HttpPost, Route("[action]")]
        //public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand createUserCommand)
        //{
        //    UserDto user = await _mediator.Send(createUserCommand);
        //    return Ok(user);
        //}
    }
}
