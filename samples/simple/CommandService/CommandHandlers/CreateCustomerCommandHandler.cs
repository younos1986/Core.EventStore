using System;
using System.Threading;
using System.Threading.Tasks;
using CommandService.Commands;
using CommandService.Dtos;
using Core.EventStore.Dependencies;
using IntegrationEvents;
using MediatR;

namespace CommandService.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        readonly IEventStoreDbContext _eventStoreDbContext;

        public CreateCustomerCommandHandler(
             IEventStoreDbContext eventStoreDbContext
            )
        {
            _eventStoreDbContext = eventStoreDbContext;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
        {
            // Raising Event ...
            var @event = new CustomerCreated(Guid.NewGuid(), cmd.FirstName, cmd.LastName, DateTime.UtcNow);
            //await _mediator.Publish(@event, cancellationToken);

            var res = new CustomerDto()
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
            };

            //await Task.CompletedTask;
            await _eventStoreDbContext.AppendToStreamAsync(@event);
            return res;
        }
    }
}
