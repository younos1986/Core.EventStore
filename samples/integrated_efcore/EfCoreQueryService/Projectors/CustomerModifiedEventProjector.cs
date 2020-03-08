using Core.EventStore.Contracts;
using IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfCoreQueryService.Projectors
{
    public class CustomerModifiedEventProjector : IProjector<CustomerModified>
    {
        public async Task HandleAsync(CustomerModified integrationEvent)
        {
            await Task.CompletedTask;
        }
    }
}
