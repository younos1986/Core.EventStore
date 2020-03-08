using System;
using System.Threading;
using System.Threading.Tasks;
using Core.EventStore.Dependencies;
using EfCoreCommandService.Commands;
using EfCoreCommandService.Dtos;
using IntegrationEvents;
using MediatR;

namespace EfCoreCommandService.CommandHandlers
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
            var @event = new CustomerCreated(cmd.FirstName, cmd.LastName, DateTime.UtcNow);

            //do sth
            
            var res = new CustomerDto()
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
            };

            await _eventStoreDbContext.AppendToStreamAsync(@event);
            return res;
        }
    }
}