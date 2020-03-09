using System;
using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;

namespace EfCoreQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreated>
    {

        public static CustomerCreated _CustomerCreated;
        
        
        public CustomerCreatedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            _CustomerCreated = integrationEvent;
            await Task.CompletedTask;
        }
    }
}
