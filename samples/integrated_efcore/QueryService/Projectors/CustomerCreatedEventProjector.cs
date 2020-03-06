using System;
using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;

namespace QueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreated>
    {
        public CustomerCreatedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            await Task.CompletedTask;
        }
    }
}
