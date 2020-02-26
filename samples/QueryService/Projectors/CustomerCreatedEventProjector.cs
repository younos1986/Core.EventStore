using Core.EventStore.Contracts;
using IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreated>
    {
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            await Task.CompletedTask;
        }
    }
}
