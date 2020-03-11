using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;

namespace PostgresQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreatedForPostgres>
    {

        public static CustomerCreatedForPostgres _CustomerCreated;
        
        
        public CustomerCreatedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreatedForPostgres integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            _CustomerCreated = integrationEvent;
            await Task.CompletedTask;
        }
    }
}
