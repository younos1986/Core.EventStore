using Core.EventStore.Contracts;
using IntegrationEvents;
using System;
using System.Threading.Tasks;

namespace QueryService.Projectors
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
