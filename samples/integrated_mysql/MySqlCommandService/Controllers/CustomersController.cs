﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MySqlCommandService.Commands;
using MySqlCommandService.Dtos;

namespace MySqlCommandService.Controllers
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
            if (Guid.Empty == createCustomerCommand.Id)
                createCustomerCommand.Id = Guid.NewGuid();
            
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
