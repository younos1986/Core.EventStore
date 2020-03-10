using System;
using System.Threading;
using System.Threading.Tasks;
using Core.EventStore.Dependencies;
using IntegrationEvents;
using MediatR;
using MySqlCommandService.Commands;
using MySqlCommandService.Dtos;

namespace MySqlCommandService.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        readonly IEventStoreDbContext _eventStoreDbContext;

        public CreateCustomerCommandHandler(IEventStoreDbContext eventStoreDbContext)
        {
            _eventStoreDbContext = eventStoreDbContext;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
        {
            var @event = new CustomerCreated(cmd.Id, cmd.FirstName, cmd.LastName, DateTime.UtcNow);

            //do sth
            
            var res = new CustomerDto()
            {
                Id =@event.Id,
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                CreatedOn = @event.CreatedOn

            };

            await _eventStoreDbContext.AppendToStreamAsync(@event);
            return res;
        }
    }
}