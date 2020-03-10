using System;
using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;

namespace EfCoreQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreatedForEfCore>
    {

        public static CustomerCreatedForEfCore _CustomerCreated;
        
        
        public CustomerCreatedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreatedForEfCore integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            _CustomerCreated = integrationEvent;
            await Task.CompletedTask;
        }
    }
}
