using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;

namespace MySqlQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreatedForMySql>
    {

        public static CustomerCreatedForMySql _CustomerCreated;
        
        
        public CustomerCreatedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreatedForMySql integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            _CustomerCreated = integrationEvent;
            await Task.CompletedTask;
        }
    }
}
