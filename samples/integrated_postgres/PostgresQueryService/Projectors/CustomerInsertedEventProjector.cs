using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;

namespace PostgresQueryService.Projectors
{
    public class CustomerInsertedEventProjector : IProjector<CustomerCreated>
    {
        public CustomerInsertedEventProjector()
        {
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            Console.WriteLine(integrationEvent);
            await Task.CompletedTask;
        }
    }
}
