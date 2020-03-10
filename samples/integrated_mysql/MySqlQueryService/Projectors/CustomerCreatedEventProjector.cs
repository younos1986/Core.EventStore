using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;

namespace MySqlQueryService.Projectors
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
